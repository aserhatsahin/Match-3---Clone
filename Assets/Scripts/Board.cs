
using UnityEngine;

// Board, sahnedeki match-3 tahtasının arka plan grid'ini oluşturan ana sınıf.
public class Board : MonoBehaviour
{
    // Tahtanın yatayda kaç hücre olacağını belirler.
    public int width;

    // Her hücre için üretilecek arka plan tile prefab'ı.
    public GameObject tilePrefab;
    public GameObject[] dots;
    // Tahtanın dikeyde kaç hücre olacağını belirler.
    public int height;

    // Grid'deki bütün arka plan karelerini 2 boyutlu dizi olarak saklar.
    // Şu an sadece oluşturuluyor; ileride hücrelere erişmek için kullanılabilir.
    private BackgroundTile[,] allTiles;
    public GameObject[,] allDots;
    void Start()
    {
        // Oyun başlarken width x height boyutunda bir grid bellekte hazırlanır.
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        // Tahtanın görsel arka planını sahneye dizer.
        SetUp();
    }

    // Tahtadaki bütün arka plan karelerini tek tek üretir ve Board nesnesinin altına yerleştirir.
    private void SetUp()
    {
        // X ekseninde soldan sağa ilerler.
        for (int i = 0; i < width; i++)
        {
            // Her sütunun içinde Y ekseninde aşağıdan yukarı hücreleri oluşturur.
            for (int j = 0; j < height; j++)
            {
                // Bu tile'ın grid üzerindeki konumu.
                Vector2 tempPosition = new Vector2(i, j);

                // Prefab'ı belirtilen pozisyonda, varsayılan rotasyonla sahneye üretir.
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;

                // Hierarchy'de düzenli görünmesi için üretilen objeyi Board'un altına bağlar.
                backgroundTile.transform.parent = this.transform;

                // Inspector/Hierarchy tarafında hangi hücre olduğunu görmek kolaylaşır.
                backgroundTile.name = "( " + i + ", " + j + " )";

                // Quaternion.identity: objeyi ekstra döndürmeden, sıfır rotasyonla oluşturur.

                int dotToUse = Random.Range(0, dots.Length);
                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);

                dot.transform.parent = this.transform;
                dot.name = "( " + i + ", " + j + " )";
                allDots[i, j] = dot;
            }
        }
    }
}
