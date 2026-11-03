using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantasy_quest.enums;

namespace Fantasy_quest.Dominio
{
    // Las preguntas viven en el codigo, no en la base. El texto sale de Documentacion/pregunta.md
    // ("campesino" en el documento es clase.granjero).
    // IMPORTANTE: la base guarda la POSICION de la pregunta y de la respuesta
    // (nro_pregunta, nro_respuesta). No cambien el orden de las que ya existen:
    // las nuevas se agregan al final. Maximo 6 respuestas por pregunta.
    internal static class BancoPreguntas
    {
        public static List<Pregunta> Crear()
        {
            return new List<Pregunta>
            {
                // ---------- 1 ----------
                new Pregunta(
                    "El amanecer en Stoneland",
                    "Es una mañana despejada en Stoneland, una aldea enclavada en las faldas de las Montañas Grises, en territorio de la Casa Puñohierro. El sonido de los martillos sobre los yunques lleva horas resonando: los herreros enanos de esta región nunca descansan demasiado. Por las calles de adoquín pasan carretas cargadas de mineral de hierro con rumbo a los altos hornos de Yunquemar, a dos días al sur.\n\n" +
                    "Llegaste ayer por la tarde siguiendo la Ruta Real que baja desde Fuegoalto, capital de la Provincia Central-Ceca. En el bolsillo llevás tres Aelores de plata, la moneda acuñada por la Casa Luceronocturno en el Banco de Fuego. Con eso alcanza para desayunar, un catre y quizás algo más.\n\n" +
                    "Te sentás junto a la ventana de la posada. Afuera, el sol empieza a iluminar las cumbres. La posadera, una enana de brazos fornidos, te mira y aguarda.",
                    "¿Qué desayunarías?",
                    new List<Respuesta>
                    {
                        new Respuesta("Cerveza y pan").Suma(razas.enano, 1).Suma(clase.herrero, 1),
                        new Respuesta("Carne").Suma(razas.orco, 1).Suma(razas.enano, 1).Suma(clase.guerrero, 1),
                        new Respuesta("Frutos secos").Suma(razas.elfo, 1).Suma(razas.humano, 1).Suma(clase.druida, 1),
                        new Respuesta("Pan, queso y avena").Suma(razas.humano, 1).Suma(clase.granjero, 1),
                        new Respuesta("Poción energética").Suma(razas.elfo, 1).Suma(clase.mago, 1),
                        new Respuesta("Carne de goblin").Suma(razas.orco, 1).Suma(clase.asesino, 1),
                    }),

                // ---------- 2 ----------
                new Pregunta(
                    "La procesión de los Dorados",
                    "Terminás de comer y salís a la calle principal justo cuando el pueblo cobra más vida. Una columna de jinetes avanza por el centro de Stoneland: son los Caballeros Dorados, la orden que ejecuta la voluntad de la Casa Astoria desde la Era de la Unificación. Detrás de ellos, sacerdotes con túnicas blancas y oro portan el emblema de Solis, el dios del sol. La gente los saluda desde las veredas.\n\n" +
                    "Los Caballeros Dorados escoltan, según dicen, un cargamento de Aelores hacia las bóvedas del Banco de Fuego en Fuegoalto. Es un viaje de rutina, pero su presencia siempre genera respeto en los pueblos del camino.\n\n" +
                    "Mientras los observás pasar, notás algo curioso: grabado discretamente en la pared de una herrería hay un símbolo que no pertenece a Solis. Es más antiguo. Más difícil de descifrar. Alguien en Stoneland venera a otra cosa.\n\n" +
                    "Esa noche, al borde del sueño, te preguntás en qué creés vos.",
                    "¿A qué dios rendirías culto?",
                    new List<Respuesta>
                    {
                        new Respuesta("A Solis, dios del sol y la luz").Suma(razas.humano, 1).Suma(razas.enano, 1).Suma(clase.guerrero, 1),
                        new Respuesta("A Gorlok, señor de la muerte").Suma(razas.orco, 1).Suma(clase.mago, 1),
                        new Respuesta("A Leina, diosa de la lujuria y la codicia").Suma(razas.humano, 1).Suma(clase.asesino, 1),
                        new Respuesta("A Doralbak, señor de los secretos").Suma(razas.elfo, 1).Suma(clase.mago, 1),
                        new Respuesta("A Ekrolf, primigenio de la ultraviolencia").Suma(razas.orco, 1).Suma(clase.guerrero, 1),
                        new Respuesta("A ninguno, los dioses son una mentira").Suma(razas.enano, 1).Suma(clase.granjero, 1),
                    }),

                // ---------- 3 ----------
                new Pregunta(
                    "El encargo del viajero",
                    "A la mañana siguiente, mientras te preparás para continuar el camino, un hombre se acerca a tu mesa. Es un cartógrafo de mediana edad, con la insignia desgastada de la Casa Hielofaro cosida al hombro: la orden que mantiene los faros boreales y las rutas de navegación del norte.\n\n" +
                    "Te explica que viaja hacia la Red de Faros Boreales pero que su estado de salud no le permite continuar. Lleva consigo un rollo de pergamino sellado con el blasón de la Guardia del Abismo: la milicia que custodia el borde del continente. Necesita que alguien de confianza lo lleve hasta el próximo puesto de la Guardia, a tres días al norte.\n\n" +
                    "\"No es peligroso\", dice, aunque no te termina de mirar a los ojos cuando lo dice. \"Solo es información. Coordenadas. Nombres de pasos. Nada más.\"\n\n" +
                    "Te deja el pergamino sobre la mesa y espera tu respuesta.",
                    "¿Qué harías si encontraras un secreto peligroso?",
                    new List<Respuesta>
                    {
                        new Respuesta("Lo vendería al mejor postor").Suma(razas.humano, 1).Suma(clase.asesino, 1),
                        new Respuesta("Lo guardaría para siempre, es poder").Suma(razas.elfo, 1).Suma(clase.mago, 1),
                        new Respuesta("Lo usaría para ganar influencia").Suma(razas.humano, 1).Suma(clase.guerrero, 1),
                        new Respuesta("Lo olvidaría de inmediato, no quiero problemas").Suma(razas.humano, 1).Suma(clase.granjero, 1),
                        new Respuesta("Lo compartiría con mi clan").Suma(razas.orco, 1).Suma(clase.guerrero, 1),
                        new Respuesta("Lo estudiaría obsesivamente hasta entenderlo").Suma(razas.elfo, 1).Suma(clase.druida, 1),
                    }),

                // ---------- 4 ----------
                new Pregunta(
                    "La armería de la Casa Puñohierro",
                    "Aceptes o no el encargo, algo está claro: el camino al norte no es un paseo. Antes de salir de Stoneland, entrás a la armería del pueblo, operada por un maestro herrero con el escudo de la Casa Puñohierro colgado sobre la fragua.\n\n" +
                    "El lugar huele a metal caliente y aceite. Sobre los estantes hay de todo: martillos de guerra con marcas de ceca grabadas en el mango, arcos de madera curada, dagas de filo fino, un grimorio de tapa oscura que alguien dejó en empeño hace meses. En el rincón, casi como una broma, cuelga un arado con cartel de precio.\n\n" +
                    "El herrero te dice que el acero de la Casa Puñohierro es el mejor del reino, sin encantamientos ni sigilos: metal puro, forjado a la antigua.\n\n" +
                    "Tenés Aelores para una sola compra.",
                    "¿Cuál sería tu arma preferida?",
                    new List<Respuesta>
                    {
                        new Respuesta("Un martillo de guerra forjado por mis propias manos").Suma(razas.enano, 1).Suma(clase.herrero, 1),
                        new Respuesta("Un arco largo tallado de madera milenaria").Suma(razas.elfo, 1).Suma(clase.druida, 1),
                        new Respuesta("Mis propias manos, no necesito más").Suma(razas.orco, 1).Suma(clase.guerrero, 1),
                        new Respuesta("Una daga envenenada y silenciosa").Suma(razas.humano, 1).Suma(clase.asesino, 1),
                        new Respuesta("Un grimorio lleno de hechizos arcanos").Suma(razas.elfo, 1).Suma(clase.mago, 1),
                        new Respuesta("Un arado, las armas son para tontos").Suma(razas.humano, 1).Suma(clase.granjero, 1),
                    }),

                // ---------- 5 ----------
                new Pregunta(
                    "La encrucijada de las tres rutas",
                    "Salís de Stoneland al amanecer. El camino principal se divide en una encrucijada marcada por un hito de piedra labrada, obra de la Casa Valdemonte, que administra los pasos y portazgos de la región.\n\n" +
                    "Tres rutas. Tres mundos distintos.\n\n" +
                    "Al norte, la Ruta de los Faros lleva hacia los territorios fríos de la Casa Sanguíneas y la Guardia del Abismo. Al este, el camino desciende hacia los puertos del Mar Fento, donde los barcos de la Casa Mareabrava conectan con las rutas legendarias que alguna vez usaron los elfos antes del exilio. Al oeste, la senda de tierra roja conduce al Valle de la Luna, la gran isla saturada de magia antigua donde los orcos fundaron sus comunidades después de la Gran Separación.\n\n" +
                    "Y al sur, claro, siempre está Fuegoalto y la comodidad de la civilización.\n\n" +
                    "El hito de piedra tiene grabada la fecha de su construcción: año 312, Era II, Alivio. Hace más de doscientos años que este cruce existe. Muchos viajeros eligieron aquí.\n\n" +
                    "Ahora te toca a vos.",
                    "¿Dónde vivirías?",
                    new List<Respuesta>
                    {
                        new Respuesta("En las profundidades de una montaña, junto a mis forjas").Suma(razas.enano, 1).Suma(clase.herrero, 1),
                        new Respuesta("En los bosques del este, más allá del mar Fento").Suma(razas.elfo, 1).Suma(clase.druida, 1),
                        new Respuesta("En el Valle de la Luna, rodeado de magia antigua").Suma(razas.orco, 1).Suma(clase.mago, 1),
                        new Respuesta("En una aldea tranquila, lejos de la guerra").Suma(razas.humano, 1).Suma(clase.granjero, 1),
                        new Respuesta("En las sombras de una gran ciudad").Suma(razas.humano, 1).Suma(clase.asesino, 1),
                        new Respuesta("En el frente de batalla, donde empieza el peligro").Suma(razas.orco, 1).Suma(clase.guerrero, 1),
                    }),

                // ---------- 6 ----------
                new Pregunta(
                    "El encuentro en el camino",
                    "Llevás dos días caminando cuando escuchás voces delante. En un tramo angosto del camino, rodeado de rocas, un grupo de viajeros está siendo detenido por tres hombres armados que exigen un \"peaje no oficial\". No son soldados de ninguna casa noble. Son oportunistas que aprovechan que la Guardia del Abismo está ocupada más al norte.\n\n" +
                    "Los viajeros son un mercader y su asistente, claramente asustados. El mercader lleva el sello de la Casa Luceronocturno en su cinturón: trabaja para el Banco de Fuego.\n\n" +
                    "Los tres hombres todavía no te vieron. Podés intervenir, rodear, o seguir de largo sin mezclarte. En un momento la situación se resuelve a tu favor, y el que parecía el líder del grupo termina en el piso, desarmado, mirándote desde abajo.\n\n" +
                    "Levanta una mano. La palma abierta.",
                    "¿Qué harías si un enemigo caído te pide clemencia?",
                    new List<Respuesta>
                    {
                        new Respuesta("Lo remato sin pensarlo, los sentimentalismos matan").Suma(razas.orco, 1).Suma(clase.guerrero, 1),
                        new Respuesta("Lo capturo vivo y pido rescate").Suma(razas.humano, 1).Suma(clase.asesino, 1),
                        new Respuesta("Lo curo y lo libero, el honor lo exige").Suma(razas.humano, 1).Suma(clase.guerrero, 1),
                        new Respuesta("Lo ignoro y sigo mi camino, no es asunto mío").Suma(razas.enano, 1).Suma(clase.granjero, 1),
                        new Respuesta("Estudio su mente antes de decidir").Suma(razas.elfo, 1).Suma(clase.mago, 1),
                        new Respuesta("Le ofrezco agua y escucho su historia").Suma(razas.elfo, 1).Suma(clase.druida, 1),
                    }),

                // ---------- 7 ----------
                new Pregunta(
                    "Los restos de la Era de la Magia",
                    "El mercader agradecido te invita a caminar junto a su grupo por el tramo siguiente. Mientras avanzan, atraviesan una zona que el mercader llama \"tierra vieja\": un valle donde los árboles crecen en espiral, las piedras flotan apenas a medio palmo del suelo y el aire vibra con una calidez que no tiene ninguna fuente visible.\n\n" +
                    "\"Restos de la Era II\", explica. \"Cuando la magia todavía corría libre por Aeloria. Antes de que la Casa Astoria y los Caballeros Dorados impulsaran la Era Antimagia. Ahora la magia está regulada. Solo los orcos del Valle de la Luna la practican con libertad, y algunos magos con licencia de las casas nobles.\"\n\n" +
                    "El mercader señala hacia el fondo del valle, donde una figura encapuchada camina entre los árboles en espiral, trazando patrones con las manos. Los árboles responden.\n\n" +
                    "\"Mago con licencia\", dice él. \"O eso esperemos.\"",
                    "¿Qué opinás de la magia?",
                    new List<Respuesta>
                    {
                        new Respuesta("Es una herramienta como cualquier otra").Suma(razas.humano, 1).Suma(clase.mago, 1),
                        new Respuesta("Es el camino verdadero hacia el conocimiento").Suma(razas.elfo, 1).Suma(clase.mago, 1),
                        new Respuesta("Me da desconfianza, prefiero el acero").Suma(razas.enano, 1).Suma(clase.guerrero, 1),
                        new Respuesta("La usaría solo si no queda otra opción").Suma(razas.humano, 1).Suma(clase.granjero, 1),
                        new Respuesta("Fluye en mi sangre desde el exilio en el Valle de la Luna").Suma(razas.orco, 1).Suma(clase.mago, 1),
                        new Respuesta("Es peligrosa, vi lo que hacen los Primigenios con ella").Suma(razas.enano, 1).Suma(clase.herrero, 1),
                    }),

                // ---------- 8 ----------
                new Pregunta(
                    "La misión más grande",
                    "Llegás a la ciudad de paso de Puerta Norte, un nudo de rutas administrado por la Casa Puñopiedra. Tiene puentes sólidos, posadas limpias y un tablero de anuncios donde la Guardia del Abismo pega sus convocatorias.\n\n" +
                    "En la posada, un mensajero con el blasón de la Casa Astoria te busca por nombre. Trae una carta con el sello de la Biblioteca Real, que funciona bajo el Coloso Primigenio, la estructura más antigua de Aeloria.\n\n" +
                    "La carta es breve. Dicen que el pergamino que llevás —o el camino que estás tomando, o simplemente quien sos— es relevante para algo que están investigando. Te piden que te presentes en Fuegoalto. El viaje desde aquí lleva semanas. Cruza territorios de varias casas nobles, un paso de montaña bajo control de Valdemonte, y al menos un tramo de ruta sin custodia conocida.\n\n" +
                    "El mensajero ya se fue antes de que termines de leer.",
                    "¿Qué llevarías en un viaje largo?",
                    new List<Respuesta>
                    {
                        new Respuesta("Herramientas de forja y metal bruto").Suma(razas.enano, 1).Suma(clase.herrero, 1),
                        new Respuesta("Hierbas medicinales y semillas del bosque").Suma(razas.elfo, 1).Suma(clase.druida, 1),
                        new Respuesta("Un grimorio y tinta para escribirlo todo").Suma(razas.elfo, 1).Suma(clase.mago, 1),
                        new Respuesta("Solo lo puesto y mi cuchillo").Suma(razas.orco, 1).Suma(clase.asesino, 1),
                        new Respuesta("Provisiones para una semana y un buen mapa").Suma(razas.humano, 1).Suma(clase.granjero, 1),
                        new Respuesta("Mis armas y nada más, todo lo demás se consigue").Suma(razas.orco, 1).Suma(clase.guerrero, 1),
                    }),

                // ---------- 9 ----------
                new Pregunta(
                    "La despedida en el cruce",
                    "Durante el camino hacia Fuegoalto, viajaste un buen tramo junto a alguien que apareció en la posada de Puerta Norte: un compañero de ruta con quien compartiste fogones, raciones y conversaciones nocturnas sobre Aeloria, sus casas, sus dioses y su historia.\n\n" +
                    "Pero los caminos se separan. Al llegar al cruce de la Provincia Central-Ceca, tu compañero debe tomar la ruta hacia el este, hacia los puertos del Mar Fento. Tiene sus propios asuntos, su propio destino. Se despide con un apretón de manos y una frase que no olvidás fácil: \"En Aeloria los caminos siempre vuelven a cruzarse.\"\n\n" +
                    "Lo ves alejarse por la ruta del este hasta que desaparece en la primera curva. Seguís solo. Fuegoalto todavía está lejos.",
                    "¿Cómo reaccionarías ante la muerte de un aliado?",
                    new List<Respuesta>
                    {
                        new Respuesta("Juro venganza y la cumplo sin importar el precio").Suma(razas.orco, 1).Suma(clase.guerrero, 1),
                        new Respuesta("Lo lloro en silencio y sigo adelante solo").Suma(razas.elfo, 1).Suma(clase.druida, 1),
                        new Respuesta("Aprendo de su muerte para no cometer el mismo error").Suma(razas.humano, 1).Suma(clase.mago, 1),
                        new Respuesta("No hay tiempo para el duelo, la guerra no espera").Suma(razas.enano, 1).Suma(clase.guerrero, 1),
                        new Respuesta("Recuerdo sus secretos y los guardo como un tesoro").Suma(razas.elfo, 1).Suma(clase.asesino, 1),
                        new Respuesta("Acepto que la muerte es parte del camino").Suma(razas.orco, 1).Suma(clase.mago, 1),
                    }),

                // ---------- 10 ----------
                new Pregunta(
                    "A la vista de Fuegoalto",
                    "Después de semanas de camino, la ves por primera vez desde una loma: Fuegoalto, capital de la Provincia Central-Ceca, sede de la Casa Luceronocturno y hogar del Banco de Fuego. Las torres de la ceca brillan al sol de la tarde. Desde aquí se ven las chimeneas de los talleres, las bóvedas de piedra gris y los estandartes dorados que ondean sobre el edificio principal.\n\n" +
                    "Es la ciudad más importante que vas a pisar en mucho tiempo.\n\n" +
                    "Pero antes de bajar la loma, te detenés un momento. Mirás atrás: el camino que recorriste, las Montañas Grises en el horizonte, el cruce donde te despediste de tu compañero. Pensás en el pergamino, en el mercader del Banco de Fuego, en los Caballeros Dorados que viste en Stoneland.\n\n" +
                    "¿Por qué llegaste hasta acá? No es una pregunta fácil.",
                    "¿Qué te mueve a actuar?",
                    new List<Respuesta>
                    {
                        new Respuesta("La supervivencia de mi familia y mi tierra").Suma(razas.humano, 1).Suma(clase.granjero, 1),
                        new Respuesta("El honor y la gloria en el combate").Suma(razas.enano, 1).Suma(clase.guerrero, 1),
                        new Respuesta("El conocimiento que aún no he alcanzado").Suma(razas.elfo, 1).Suma(clase.mago, 1),
                        new Respuesta("El oro y el poder que trae consigo").Suma(razas.humano, 1).Suma(clase.asesino, 1),
                        new Respuesta("La protección de la naturaleza y el equilibrio del mundo").Suma(razas.elfo, 1).Suma(clase.druida, 1),
                        new Respuesta("La sangre y el caos, hay belleza en la destrucción").Suma(razas.orco, 1).Suma(clase.guerrero, 1),
                    }),

                // ---------- 11 ----------
                new Pregunta(
                    "El visitante del camposanto",
                    "En las afueras de Fuegoalto hay un pequeño camposanto con lápidas que registran nombres de todas las razas: enanos, humanos, algún elfo que murió lejos de casa. Pasás por ahí al anochecer, casi por casualidad, y terminás sentándote en un banco de piedra frente a una lápida que no podés leer bien con tan poca luz.\n\n" +
                    "Cuando levantás la vista, hay alguien sentado al otro extremo del banco.\n\n" +
                    "No lo escuchaste llegar. No hay pasos, no hay pisadas en la tierra. Solo está ahí, con una apariencia completamente ordinaria: podría ser un funcionario de la ceca, un viajero, un estudiante de la Biblioteca Real. Pero algo no cierra. La forma en que te mira es demasiado tranquila. Demasiado paciente. Como si llevara siglos esperando exactamente este momento.\n\n" +
                    "\"Sabés quién soy\", dice. No es una pregunta.\n\n" +
                    "En Aeloria se aprende desde chico que hay seis Primigenios, hermanos de Solis, que existen desde antes del tiempo. Uno de ellos camina entre los mortales sin que nadie lo note. Siembra confusión, hace preguntas que no tienen respuesta fácil, y a veces, dicen, ofrece tratos.\n\n" +
                    "Vaker. El dios de los secretos que se hacen preguntas.\n\n" +
                    "\"Llegaste muy lejos para alguien que salió de Stoneland sin un plan claro\", dice. \"Tengo algo que podría interesarte.\"",
                    "¿Qué harías si Vaker te ofreciera un trato?",
                    new List<Respuesta>
                    {
                        new Respuesta("Lo rechazaría de inmediato, ningún pacto con los Primigenios").Suma(razas.enano, 1).Suma(clase.guerrero, 1),
                        new Respuesta("Escucharía la oferta antes de decidir").Suma(razas.humano, 1).Suma(clase.mago, 1),
                        new Respuesta("Aceptaría si el precio es conveniente").Suma(razas.humano, 1).Suma(clase.asesino, 1),
                        new Respuesta("Huiría sin mirar atrás").Suma(razas.humano, 1).Suma(clase.granjero, 1),
                        new Respuesta("Lo estudiaría como fenómeno arcano único").Suma(razas.elfo, 1).Suma(clase.mago, 1),
                        new Respuesta("Negociaría, todo tiene su precio en Aeloria").Suma(razas.orco, 1).Suma(clase.asesino, 1),
                    }),

                // ---------- 12 ----------
                new Pregunta(
                    "La primera noche en Fuegoalto",
                    "Entrás a Fuegoalto al día siguiente, con el encuentro del camposanto todavía dando vueltas en la cabeza. La ciudad es grande y ruidosa: carretas del Banco de Fuego cruzan las calles principales, los talleres de la Casa Luceronocturno trabajan hasta tarde y el olor a metal fundido llena los barrios cercanos a la ceca. En el centro, el edificio del Banco de Fuego tiene guardias con el escudo solar de la Casa Astoria en la armadura.\n\n" +
                    "Tenés la dirección de la Biblioteca Real para el día siguiente. Por ahora, la noche es tuya.",
                    "¿Cómo pasarías tu primera noche en una ciudad nueva?",
                    new List<Respuesta>
                    {
                        new Respuesta("Recorriendo los talleres y aprendiendo de los artesanos locales").Suma(razas.enano, 1).Suma(clase.herrero, 1),
                        new Respuesta("Buscando una plaza con árboles, lejos del ruido").Suma(razas.elfo, 1).Suma(clase.druida, 1),
                        new Respuesta("En la biblioteca pública, leyendo todo lo que pueda").Suma(razas.elfo, 1).Suma(clase.mago, 1),
                        new Respuesta("Explorando los barrios menos transitados de la ciudad").Suma(razas.humano, 1).Suma(clase.asesino, 1),
                        new Respuesta("En la taberna más concurrida, escuchando conversaciones").Suma(razas.humano, 1).Suma(clase.granjero, 1),
                        new Respuesta("Buscando el cuartel de la Guardia del Abismo para presentarme").Suma(razas.orco, 1).Suma(clase.guerrero, 1),
                    }),

                // ---------- 13 ----------
                new Pregunta(
                    "La Biblioteca Real",
                    "El Coloso Primigenio es la estructura más imponente que viste en tu vida: una torre de piedra negra que no parece haber sido construida sino encontrada, como si siempre hubiera estado ahí. La Biblioteca Real funciona en sus primeros tres niveles. Los siguientes nadie los ha mapeado del todo.\n\n" +
                    "Un archivista de la Casa Astoria te recibe, examina el pergamino del cartógrafo con mucho cuidado y finalmente levanta la vista. \"Esto confirma algo que estábamos investigando\", dice. \"Hay una misión que necesita alguien con tu perfil. Podemos ofrecerte recursos, protección y acceso a los archivos. Pero necesitamos saber qué tipo de persona sos antes de confiar en vos.\"\n\n" +
                    "Te hace una sola pregunta.",
                    "¿Qué harías si descubrieras que la historia oficial de Aeloria está incompleta?",
                    new List<Respuesta>
                    {
                        new Respuesta("Lo reportaría a la Casa Astoria de inmediato, ellos deben saberlo").Suma(razas.humano, 1).Suma(clase.guerrero, 1),
                        new Respuesta("Lo guardaría para mí, ese conocimiento tiene valor").Suma(razas.elfo, 1).Suma(clase.mago, 1),
                        new Respuesta("Lo publicaría para que todo el mundo lo sepa").Suma(razas.humano, 1).Suma(clase.granjero, 1),
                        new Respuesta("Lo usaría para negociar con quien más me convenga").Suma(razas.humano, 1).Suma(clase.asesino, 1),
                        new Respuesta("Organizaría una expedición para encontrar las pruebas que faltan").Suma(razas.orco, 1).Suma(clase.guerrero, 1),
                        new Respuesta("Pasaría años estudiándolo hasta entender cada detalle").Suma(razas.elfo, 1).Suma(clase.druida, 1),
                    }),

                // ---------- 14 ----------
                new Pregunta(
                    "La oferta de la Guardia del Abismo",
                    "Al salir de la Biblioteca Real, un capitán de la Guardia del Abismo te espera en la puerta. Lleva la capa gris con el borde negro que identifica a los veteranos del borde. La Guardia custodia el límite del continente desde la Era II, donde termina el mapa y empieza lo desconocido. No es un trabajo fácil ni glamoroso, pero nadie lo hace si no cree en algo.\n\n" +
                    "El capitán fue directo: \"Necesitamos gente. Viste cosas en el camino que la mayoría prefiere ignorar. Sabemos lo que hiciste en el tramo de Puerta Norte con el mercader de Luceronocturno. Eso dice bastante.\"\n\n" +
                    "Hace una pausa y te mira a los ojos.\n\n" +
                    "\"¿Qué hacés cuando el camino se termina y no hay mapa del otro lado?\"",
                    "¿Qué harías si llegaras al límite de lo conocido?",
                    new List<Respuesta>
                    {
                        new Respuesta("Construiría un puesto y esperaría a que lleguen otros").Suma(razas.enano, 1).Suma(clase.herrero, 1),
                        new Respuesta("Plantaría algo y vería qué crece").Suma(razas.elfo, 1).Suma(clase.druida, 1),
                        new Respuesta("Estudiaría el terreno antes de dar un solo paso más").Suma(razas.elfo, 1).Suma(clase.mago, 1),
                        new Respuesta("Cruzaría sin pensarlo, para eso llegué hasta acá").Suma(razas.orco, 1).Suma(clase.guerrero, 1),
                        new Respuesta("Buscaría la forma de sacar provecho de la situación").Suma(razas.humano, 1).Suma(clase.asesino, 1),
                        new Respuesta("Volvería a buscar refuerzos antes de seguir").Suma(razas.humano, 1).Suma(clase.granjero, 1),
                    }),

                // ---------- 15 ----------
                new Pregunta(
                    "El final del camino",
                    "Pasaron semanas desde que saliste de Stoneland. Estás sentado en una terraza de Fuegoalto con el pergamino ya entregado, la misión cumplida y una ciudad entera a tus pies. Las Torres de la ceca brillan al atardecer. Desde aquí se ve el camino por el que llegaste, serpenteando entre colinas hasta perderse en el horizonte.\n\n" +
                    "Pensás en todo lo que pasó: el desayuno en la posada de Stoneland, los Caballeros Dorados, el cartógrafo de Hielofaro, el valle de la Era II, la despedida en el cruce, Vaker en el camposanto. Cada elección te trajo hasta acá.\n\n" +
                    "El archivista de la Casa Astoria te preguntó esta mañana si pensabas quedarte en Fuegoalto o seguir viaje. Todavía no le respondiste.\n\n" +
                    "La pregunta de fondo es más simple: ¿quién sos después de todo esto?",
                    "¿Cómo te describirías al final del viaje?",
                    new List<Respuesta>
                    {
                        new Respuesta("Alguien que construye cosas que perduran").Suma(razas.enano, 1).Suma(clase.herrero, 1),
                        new Respuesta("Alguien que protege a quienes no pueden protegerse solos").Suma(razas.humano, 1).Suma(clase.guerrero, 1),
                        new Respuesta("Alguien que busca respuestas sin importar hasta dónde llevan").Suma(razas.elfo, 1).Suma(clase.mago, 1),
                        new Respuesta("Alguien que sobrevive haciendo lo que sea necesario").Suma(razas.humano, 1).Suma(clase.asesino, 1),
                        new Respuesta("Alguien que pertenece a la naturaleza, no a las ciudades").Suma(razas.elfo, 1).Suma(clase.druida, 1),
                        new Respuesta("Alguien que vive para el desafío y el siguiente horizonte").Suma(razas.orco, 1).Suma(clase.guerrero, 1),
                    }),
            };
        }
    }
}
