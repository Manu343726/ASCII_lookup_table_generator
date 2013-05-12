ASCII lookup-table generator
============================

Generador de la lookup-table de correspondencia pixels-caracter para el raster ASCII del proyecto cpp_lib32.

Descripción:
------------

Entre los test del proyecto cpp_lib32 ( https://github.com/Manu343726/cpp_lib32 ), se encuentra una sencilla implementación de un raster ASCII. 
Este proyecto automatiza el proceso de generación de las correspondencias pixels-caracter para dicho raster.

Especificaciones:
-----------------

El raster consta de tres módulos:
 - *screen.h*: Una implementación de un sencillo buffer de     
    píxeles blancos/negros.
 - *raster.h*: Implementación de los algoritmos de rasterizado para el screen anterior.
 - *ASCII_Raster.h*: Traduce el buffer de píeles a un        buffer de caracteres en ASCII-art.

La traducción buffer_píxeles a buffer_caracteres está configurada de manera que cada caracter ocupa un número de píxeles específico. Por ejemplo, un carácter podría ocupar 2x2 pixels, lo que haría necesarios 16 caracteres para representar todas las combinaciones de píxeles posibles.

El cometido de éste proyecto es generar el conjunto de combinaciones de píxeles necesarios para una configuración dada, y calcular los valores de brillo del conjunto de caracteres ASCII para hallar la correspondencia caracter-pixels. 
En otras palabras, encontrar los caracteres óptimos para representar cada una de las posibles combinaciones de píxeles.