"""Compila el manual de usuario y la doc de interfaz a HTML y PDF.

Uso:  python build_docs.py
Requiere: pip install markdown  y  Microsoft Edge (para el PDF).

Las imagenes que todavia no existen salen como un recuadro con la ruta esperada.
Se listan en PENDIENTES.txt dentro de cada carpeta de imagenes; al subir todas,
volver a correr el script y el archivo se borra solo.
"""

import html
import re
import shutil
import subprocess
import sys
import tempfile
from pathlib import Path

import markdown

DOCS = Path(__file__).resolve().parent

DOCUMENTOS = [
    ("Manual de usuario.md", "img_manual"),
    ("Interfaz final.md", "UI-Desing-History/V3"),
]

MINIATURAS = "_miniaturas"
PESO_MINIATURA = 300 * 1024
LADO_MINIATURA = 260

EDGE_CANDIDATOS = [
    Path(r"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"),
    Path(r"C:\Program Files\Microsoft\Edge\Application\msedge.exe"),
]

# imagen sola en su linea seguida de la leyenda en cursiva
FIGURA = re.compile(r"^!\[(?P<alt>[^\]]*)\]\((?P<src>[^)]+)\)\n\*(?P<leyenda>.+)\*$", re.MULTILINE)

CSS = """
:root { --tinta:#2a1d10; --dorado:#9a6b16; --pergamino:#fbf6ea; --borde:#d9c9a3; }
* { box-sizing: border-box; }
body { margin:0; background:var(--pergamino); color:var(--tinta);
       font: 15px/1.55 "Palatino Linotype", Palatino, Georgia, serif; }
main { max-width: 900px; margin: 0 auto; padding: 40px 24px 60px; }
h1 { font-size: 2.1em; color: var(--dorado); border-bottom: 3px double var(--borde); padding-bottom: .3em; margin-top: 0; }
h1 + p { color:#6b5a44; margin-top:-.4em; }
h2 { color: var(--dorado); border-bottom: 1px solid var(--borde); padding-bottom: .2em; margin-top: 2em; }
h3 { margin-top: 1.6em; }
nav.indice { background:#f3ead3; border:1px solid var(--borde); padding: 12px 20px; border-radius: 6px; }
nav.indice ol { margin: .3em 0; padding-left: 1.3em; columns: 2; }
nav.indice a { color: var(--tinta); text-decoration: none; }
table { border-collapse: collapse; width: 100%; margin: 1em 0; font-size: .93em; }
th, td { border: 1px solid var(--borde); padding: 6px 9px; vertical-align: top; text-align: left; }
th { background: #efe2c2; }
tr:nth-child(even) td { background: #fffaf0; }
td img { max-width: 100%; max-height: 130px; display:block; margin:auto; }
code { background:#efe6cf; padding: 1px 5px; border-radius: 3px; font-size: .9em; }
pre { background:#efe6cf; padding: 12px 14px; border-radius: 6px; overflow-x: auto; line-height: 1.35; }
pre code { background:none; padding:0; }
blockquote { border-left: 4px solid var(--borde); margin-left: 0; padding-left: 14px; color:#5b4a35; }
figure { margin: 1.4em 0; text-align: center; page-break-inside: avoid; }
figure img { max-width: 100%; border: 1px solid var(--borde); border-radius: 4px; box-shadow: 0 2px 8px rgba(0,0,0,.12); }
figcaption { font-size: .88em; color:#6b5a44; margin-top: .5em; font-style: italic; }
.pendiente { border: 2px dashed #b8955a; background: repeating-linear-gradient(45deg,#f7efdc,#f7efdc 12px,#f2e7cd 12px,#f2e7cd 24px);
             border-radius: 6px; padding: 34px 20px; color: #7a5a24; }
.pendiente strong { display:block; font-size: 1.05em; margin-bottom: .3em; }
.pendiente code { background: #fff8e6; }
@media print {
  body { background: #fff; font-size: 11pt; }
  main { max-width: none; padding: 0; }
  h2 { page-break-after: avoid; }
  table, figure { page-break-inside: avoid; }
  nav.indice ol { columns: 2; }
}
@page { size: A4; margin: 18mm 16mm; }
"""


def figura(m, carpeta_doc, faltantes):
    src, alt, leyenda = m["src"], m["alt"], m["leyenda"]
    ruta = (carpeta_doc / src).resolve()
    leyenda_html = markdown.markdown(leyenda).removeprefix("<p>").removesuffix("</p>")
    if ruta.exists():
        cuerpo = f'<img src="{html.escape(src)}" alt="{html.escape(alt)}">'
    else:
        faltantes.append((src, leyenda))
        cuerpo = (f'<div class="pendiente"><strong>Imagen pendiente — {html.escape(alt)}</strong>'
                  f'Guardar la captura como <code>{html.escape(src)}</code></div>')
    return f'<figure>{cuerpo}<figcaption>{leyenda_html}</figcaption></figure>'


def usar_miniaturas(cuerpo, carpeta_doc):
    """Los PNG de razas pesan hasta 1.6 MB: sin esto el PDF de interfaz pasa los 20 MB."""
    try:
        from PIL import Image
    except ImportError:
        return cuerpo

    destino = DOCS / MINIATURAS
    destino.mkdir(exist_ok=True)

    def reemplazo(m):
        src = html.unescape(m[1])
        original = (carpeta_doc / src).resolve()
        if not original.exists() or original.stat().st_size < PESO_MINIATURA:
            return m[0]
        mini = destino / f"{original.parent.name}_{original.stem}.webp"
        if not mini.exists() or mini.stat().st_mtime < original.stat().st_mtime:
            with Image.open(original) as imagen:
                imagen.thumbnail((LADO_MINIATURA, LADO_MINIATURA))
                imagen.save(mini, quality=82, method=6)
        relativa = Path(MINIATURAS, mini.name).as_posix()
        return m[0].replace(m[1], html.escape(relativa))

    return re.sub(r'<img [^>]*?src="([^"]+)"', reemplazo, cuerpo)


def anclar_titulos(cuerpo):
    indice = []

    def reemplazo(m):
        texto = re.sub(r"<[^>]+>", "", m[1])
        ancla = re.sub(r"[^a-z0-9]+", "-", texto.lower()).strip("-")
        indice.append((ancla, texto))
        return f'<h2 id="{ancla}">{m[1]}</h2>'

    cuerpo = re.sub(r"<h2>(.*?)</h2>", reemplazo, cuerpo)
    items = "".join(f'<li><a href="#{a}">{t}</a></li>' for a, t in indice)
    nav = f'<nav class="indice"><strong>Contenido</strong><ol>{items}</ol></nav>'
    # el indice va despues del titulo y la linea de version
    return re.sub(r"(</h1>\s*<p>.*?</p>)", lambda m: m[1] + nav, cuerpo, count=1, flags=re.S)


def compilar_html(md_path, carpeta_img):
    texto = md_path.read_text(encoding="utf-8")
    faltantes = []
    texto = FIGURA.sub(lambda m: figura(m, md_path.parent, faltantes), texto)
    cuerpo = markdown.markdown(texto, extensions=["tables", "fenced_code", "sane_lists"])
    cuerpo = usar_miniaturas(anclar_titulos(cuerpo), md_path.parent)
    titulo = re.search(r"<h1>(.*?)</h1>", cuerpo)
    titulo = re.sub(r"<[^>]+>", "", titulo[1]) if titulo else md_path.stem

    salida = md_path.with_suffix(".html")
    salida.write_text(
        f'<!doctype html>\n<html lang="es"><head><meta charset="utf-8">'
        f'<meta name="viewport" content="width=device-width, initial-scale=1">'
        f'<title>{html.escape(titulo)}</title><style>{CSS}</style></head>'
        f'<body><main>{cuerpo}</main></body></html>\n',
        encoding="utf-8")

    carpeta = DOCS / carpeta_img
    carpeta.mkdir(parents=True, exist_ok=True)
    pendientes = carpeta / "PENDIENTES.txt"
    if faltantes:
        lineas = [f"Capturas que faltan para '{md_path.name}'. Guardarlas con este nombre exacto (PNG):", ""]
        for src, leyenda in faltantes:
            lineas.append(Path(src).name)
            lineas.append("    " + re.sub(r"[*`\\]", "", leyenda))
        pendientes.write_text("\n".join(lineas) + "\n", encoding="utf-8")
    elif pendientes.exists():
        pendientes.unlink()
    return salida, faltantes


def buscar_edge():
    for candidato in EDGE_CANDIDATOS:
        if candidato.exists():
            return candidato
    encontrado = shutil.which("msedge")
    return Path(encontrado) if encontrado else None


def compilar_pdf(edge, html_path):
    pdf = html_path.with_suffix(".pdf")
    # perfil temporal: si Edge ya esta abierto, headless con el perfil normal no imprime
    with tempfile.TemporaryDirectory() as perfil:
        subprocess.run([str(edge), "--headless=new", "--disable-gpu", "--no-first-run",
                        f"--user-data-dir={perfil}", "--no-pdf-header-footer",
                        f"--print-to-pdf={pdf}", html_path.as_uri()],
                       check=True, timeout=180, capture_output=True)
    if not pdf.exists():
        raise RuntimeError(f"Edge no genero {pdf.name}")
    return pdf


def main():
    edge = buscar_edge()
    if edge is None:
        print("Aviso: no se encontro Microsoft Edge, solo se genera el HTML.")

    for nombre, carpeta_img in DOCUMENTOS:
        md_path = DOCS / nombre
        html_path, faltantes = compilar_html(md_path, carpeta_img)
        print(f"{html_path.name}: ok ({len(faltantes)} imagenes pendientes)")
        if edge:
            pdf = compilar_pdf(edge, html_path)
            print(f"{pdf.name}: ok ({pdf.stat().st_size // 1024} KB)")


if __name__ == "__main__":
    sys.exit(main())
