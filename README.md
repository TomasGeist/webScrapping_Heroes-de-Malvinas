# App scrapper de tabla Héroes de Malvinas.
En este repositorio se desarrolla una aplicación que utiliza la librería [HtmlAgilityPack](https://html-agility-pack.net/). para hacer un scrapping de una tabla que contiene los datos de los Héroes que combatieron y Malvinas y Veteranos de Guerra.

---------
## Índice

- [Clases y metodos utilizados de Html Agility Pack](#clases-y-metodos-utilizados-de-html-agility-pack)
- [Clases y metodos utilizados de .NET](#clases-y-metodos-utilizados-de-.net)
- [Funcionamiento del codigo](#funcionamiento-del-codigo)
- [Linq](#linq) 

-------------
## Clases y metodos utilizados de Html Agility Pack
<details open>
<summary>Breve introducción a la librería</summary>
Es un analizador HTML escrito en C# para leer/escribir DOM. Basicamente se puede extraer elementos de una página web y utilizarlos en tu programa.
</details>
<details >
<summary>¿El scrapping es Ilegal?</summary>
En este caso la página scrappeada es del gobierno y aclara específicamente "Los contenidos del Ministerio de Defensa están licenciados bajo Creative Commons Reconocimiento 2.5 Argentina License", esta licencia permite utilizar su contenido.

- Para más información: [Creative Commons](https://creativecommons.org/licenses/by/2.5/ar/) 

- Página utilizada: [Ministerio de defensa](https://www.veteranos.mindef.gov.ar/index.php)
</details>

<details >
<summary>Clases utilizadas</summary>

- **htmlDocument**: Es una clase proporcionada por la biblioteca HtmlAgilityPack en .NET que se utiliza para analizar y manipular documentos HTML.
```
var htmlDocument = new HtmlDocument();
```
Sobre este objeto trabajo con el metodo ```Load()``` que carga el HTML obtenido de la página web en el objeto htmlDocument.


- **DocumentNode**: Esta propiedad devuelve el nodo raíz del documento HTML. A partir de este nodo podés navegar por toda la estructura del documento.

- **CssSelect()**: Este método permite seleccionar elementos utilizando selectores CSS y devuelve una colección de nodos que coinciden con el selector CSS especificado.
```
var heroesElement = htmlDocument.DocumentNode.CssSelect("td").Select(x=> x.InnerText);
```
</details>


## Clases y metodos utilizados de .NET
- [**httpClient**](https://learn.microsoft.com/es-es/uwp/api/windows.web.http.httpclient?view=winrt-22621): es una clase en el espacio de nombres System.Net.Http que proporciona una forma eficiente de enviar y recibir solicitudes HTTP en .NET.
```
var httpCliente = new HttpClient();
```
- [**GetStringAsync**()](https://learn.microsoft.com/es-es/dotnet/api/system.net.http.httpclient.getstringasync?view=net-8.0): Esta función esta dentro de la clase HttpClient, y sirve para traer el contenido de la url que estamos mencionando.
```
var html = httpCliente.GetStringAsync(url).Result;
```
Por otro lado el .Result es un metodo de la clase TASK de .NET que espera el resultado de la peticion HTTP asincrona.


- [**getRange()**](https://learn.microsoft.com/es-es/dotnet/api/system.collections.arraylist.getrange?view=net-8.0): es un método que se utiliza para obtener una sublista de elementos de una lista existente, basada en un índice inicial y una longitud especificada. Este método está disponible en las listas genéricas en .NET.
- [**Math.Min()**](https://learn.microsoft.com/es-es/dotnet/api/system.math.min?view=net-8.0): Este es un método estático de la clase Math en .NET que toma dos valores numéricos como argumentos y devuelve el valor más pequeño de los dos.

```
partes.Add(lst.GetRange(i, Math.Min(6, lst.Count - i)));
```

## Funcionamiento del codigo
<details>
<summary>URL</summary>


```
string url = "https://www.veteranos.mindef.gov.ar/index.php";
```
Este bloque de codigo declara la url de la página que va a ser analizada. 

**Si se requiere analizar otra página web es necesario cambiar la logica del selector CSS.**
</details>

<details>
<summary>Lista de Héroes</summary>

- La variable heroesElement contiene una colección de cadenas que tomó anteriormente del <"td">. Cada uno simboliza un campo de la tabla.

```
var lst = new List<String>();

            heroesElement.ToList().ForEach(e=>
            {
                lst.Add(e.ToString());
            });
```
Con este código se crea una nueva lista de strings y se convierte cada elemento de la colección heroesElement en un string y se agrega a la lista "lst".

>En este momento hay una lista de +23.000 strings con datos ordenados.

>El formato de la lista es:

1. Nombre Completo [0],
2. Documento [1],
3. Fuerza [2],
4. Grado [3],
5. Vive [4],
6. Condicion [5]

>y apartir de aquí comienza el proximo heroe:

7. Nombre Completo [6]
8. Documento [7],
9. ...


- **La Lista de listas**:
```
List<List<string>> partes = new List<List<string>>();
            for (int i = 0; i < lst.Count; i += 6) 
            {
                partes.Add(lst.GetRange(i, Math.Min(6, lst.Count - i)));
            }
```

Se crea la lista "partes" que esta conformada por listas de 6 strings tomados de "lst". De esta manera se pueden dividir los datos de cada héroe y separarlos.

</details>


<details>
<summary>Creación de objetos e insert en la BDD</summary>

1. El foreach recorre la lista de listas y por cada parte de 6 strings crea un objeto de la clase Heroe y le asigna los datos que estan dentro de la lista en cuestión. 

2. Por ultimo se agrega el objeto a la base de datos y continúa con el siguiente.

```
foreach (var parte in partes)
{
     Heroe oHeroe = new Heroe
    {

        NombreCompleto = parte[0],
        Documento = parte[1],
        Fuerza = parte[2],
        Grado = parte[3],
        Vive = parte[4],
        Condicion = parte[5]

    };

     using (DbHeroesdemalvinasContext db = new DbHeroesdemalvinasContext())
    {

        await db.Heroes.AddAsync(oHeroe);
        await db.SaveChangesAsync();
    }

}
```

</details>

## Linq



<details>

<summary>¿Que es LINQ?</summary>

Es una herramienta que permite realizar consultas sobre datos de una manera similar a cómo se consultan las bases de datos.

* [Documentación de Linq](https://learn.microsoft.com/es-es/dotnet/csharp/linq/get-started/introduction-to-linq-queries)



</details>


<details>

<summary>¿Como esta utilizado en el proyecto?</summary>

```
var heroesElement = htmlDocument.DocumentNode.CssSelect("td").Select(x=> x.InnerText);
```

El **.Select** utiliza Linq.
</details>

