using SQLite;

namespace Programadepaises1.Models;

[SQLite.Table("PaisesFavoritos")]
public class PaisFavorito
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Unique]
    public string NombreComun { get; set; } = string.Empty;

    public string BanderaUrl { get; set; } = string.Empty;

    public string Capital { get; set; } = string.Empty;

    public long Poblacion { get; set; }

    public string Region { get; set; } = string.Empty;

    public DateTime FechaAgregado { get; set; } = DateTime.Now;

    // Constructor para convertir desde PaisModel
    public static PaisFavorito FromPaisModel(PaisModel pais)
    {
        return new PaisFavorito
        {
            NombreComun = pais.NombreComun,
            BanderaUrl = pais.BanderaUrl,
            Capital = pais.CapitalTexto,
            Poblacion = pais.Population,
            Region = pais.Region
        };
    }

    // Método para convertir a PaisModel
    public PaisModel ToPaisModel()
    {
        return new PaisModel
        {
            Name = new Name { Common = NombreComun },
            Flags = new Flags { Png = BanderaUrl },
            Capital = new List<string> { Capital },
            Population = Poblacion,
            Region = Region,
            EsFavorito = true
        };
    }
}
