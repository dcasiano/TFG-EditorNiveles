# Fulvinter Maker: Editor de niveles para videojuegos 2.5D

Este repositorio aloja el editor de niveles para juegos 2D y 2.5D de scroll lateral, producto del TFG ulvinter Maker: Editor de niveles para videojuegos 2.5D. Es distribuido en forma de paquete de Unity.

## Autores
David Casiano Flores

Sol Flora López Antón

## Director

Ismael Sagredo Olivenza

## Cómo utilizarlo

El repositorio contiene el proyecto de Unity completo donde se desarrolló el editor de niveles. No obstante, si quieres utilizar el editor en tu proyecto, solo necesitas descargar el archivo comprimido EditorNiveles.zip

Una vez descomprimido, debes moverlo a la carpeta Packages de tu proyecto de Unity. Tras esto, aparecerá una nueva seccción en la parte superior del editor de Unity llamada Level Editor. Allí, encontrarás el botón New Level, que creará una nueva escena con todo lo necesario para empezar a editar el nivel. En el menú Level Editor también encontrarás el botón para mostrar la paleta.

Una vez en la nueva escena, encontrarás en la jerarquía el GameObject Scenary. Clickando sobre él, accederás al inspector con diferentes opciones de personalización.

Para añadir objetos a la paleta, debes crear crear en el directorio Packages\EditorDeNiveles\Prefabs\LevelObjects tantos subdirectorios como categorías de la paleta desees. En cada uno de ellos, deberás ubicar los Prefabs de los objetos que quieres utilizar para crear tu nivel. Estos se mostrarán ordenados en categorías en la paleta.

Tras esto, en la ruta Packages\EditorDeNiveles\Editor\ScriptableObject aparecerá un ScriptableObject por cada categoría que hayas creado. En él, podrás modificar ciertos parámetros, de manera que los objetos que instancies a partir de entonces cumplan con esas propiedades. 