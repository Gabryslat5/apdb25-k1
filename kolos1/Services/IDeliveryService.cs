using kolos1.Models;

namespace kolos1.Services;

public interface IDeliveryService
{
    Task<DeliveryDTO?> GetDeliveryAsync(int delivery_id, CancellationToken cancellationToken);
    
    public enum AddDeliveryResult
    {
        Istnieje_dostawa_o_podanym_identyfikatorze,
        Nie_istnieje_klient_o_podanym_identyfikatorze,
        Nie_istnieje_kierowca_o_podanym_numerze_pracownika,
        Nie_istnieje_produkt_o_podanej_nazwie,
        Dane_nie_są_zgodne_z_walidacją
    }
    Task<AddDeliveryResult> AddDeliveryAsync(DeliveryAddDTO delivery, CancellationToken cancellationToken);
}