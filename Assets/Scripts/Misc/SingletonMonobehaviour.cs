using UnityEngine;

// Klasa abstrakcyjna, implementująca wzorzec projektowy Singleton dla klas dziedziczących z MonoBehaviour.
// Parametr generyczny T określa typ klasy dziedziczącej z MonoBehaviour.
public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    // Statyczna instancja klasy. Dostęp do instancji za pomocą T.Instance.
    private static T instance;

    // Właściwość publiczna do uzyskiwania dostępu do instancji klasy.
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    // Metoda wywoływana automatycznie podczas uruchamiania obiektu.
    // Sprawdza, czy instancja już istnieje. Jeśli nie, ustawia aktualny obiekt jako instancję. W przeciwnym razie niszczy obecny obiekt.
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            // Jeśli instancja już istnieje, niszcz obecny obiekt.
            Destroy(gameObject);
        }
    }
}
