
using UnityEngine;

public class Dot : MonoBehaviour
{
    // Bu dotun board üzerindeki sütun index'i. X koordinatı gibi düşünebilirsin.
    public int column;//x
    // Bu dotun board üzerindeki satır index'i. Y koordinatı gibi düşünebilirsin.
    public int row;//y

    // Dotun gitmek istediği hedef grid koordinatı.
    public int targetX;
    public int targetY;
    // Swipe yönünde yer değiştireceğimiz komsu dot.
    private GameObject otherDot;
    // Tüm grid'i tutan Board referansı.
    private Board board;
    // Mouse/finger ilk basıldığı dünya pozisyonu.
    private Vector2 firstTouchPosition;
    // Mouse/finger bırakıldığı dünya pozisyonu.
    private Vector2 finalTouchPosition;

    // Lerp ile hareket ettirirken kullandığımız geçici hedef pozisyon.
    private Vector2 tempPosition;
    // Swipe yönünü açı olarak tutar.
    public float swipeAngle = 0;

    void Start()
    {
        // Sahnedeki Board scriptini buluyoruz.
        board = FindFirstObjectByType<Board>();
        // Objeyi sahnede ilk açıldığı pozisyondan grid koordinatına çeviriyoruz.
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        // Başlangıçta mevcut hücre bilgisi ile hedef hücre bilgisi aynı.
        row = targetY;
        column = targetX;
    }

    // Update is called once per frame
    void Update()
    {
        // Dotun varmak istediği hücre her frame column/row'dan okunuyor.
        targetX = column;
        targetY = row;

        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            // X ekseninde hedefe doğru yumuşak hareket.
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            // Hedefe çok yaklaştıysa tam koordinata oturt.
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            // Board dizisindeki yeni hücreye bu dotu yaz.
            board.allDots[column, row] = this.gameObject;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            // Y ekseninde hedefe doğru yumuşak hareket.
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            // Hedefe çok yaklaştıysa tam koordinata oturt.
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            // Board dizisindeki yeni hücreye bu dotu yaz.
            board.allDots[column, row] = this.gameObject;
        }
    }

    private void OnMouseDown()
    {
        // Hangi dota ilk dokunduğumuzu burada yakalıyoruz.
        Debug.Log("Down: " + gameObject.name + " col=" + column + " row=" + row);
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseUp()
    {
        // Parmağın/mouse'un bırakıldığı son nokta.
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("Up: " + gameObject.name);
        // Başlangıç ve bitiş noktası arasındaki yönü hesapla.
        CalculateAngle();
    }
    void CalculateAngle()
    {
        // İki nokta arasındaki farktan swipe açısını derece cinsinden buluyoruz.
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;

        // Açıya göre hangi komşu dot ile yer değiştireceğimizi belirle.
        MovePieces();

    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width)
        {
            // Saga swipe: sağdaki komşu dotu bul.
            otherDot = board.allDots[column + 1, row];
            // Sağdaki dot bizim eski sütunumuza gelsin.
            otherDot.GetComponent<Dot>().column -= 1;
            // Bu dot bir hücre sağa gitsin.
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height)
        {
            // Yukarı swipe: üstteki komşu dotu bul.
            otherDot = board.allDots[column, row + 1];
            // Üstteki dot bizim eski satırımıza gelsin.
            otherDot.GetComponent<Dot>().row -= 1;
            // Bu dot bir hücre yukarı gitsin.
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            // Sola swipe: soldaki komşu dotu bul.
            otherDot = board.allDots[column - 1, row];
            // Soldaki dot bizim eski sütunumuza gelsin.
            otherDot.GetComponent<Dot>().column += 1;
            // Bu dot bir hücre sola gitsin.
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            // Aşağı swipe: alttaki komşu dotu bul.
            otherDot = board.allDots[column, row - 1];
            // Alttaki dot bizim eski satırımıza gelsin.
            otherDot.GetComponent<Dot>().row += 1;
            // Bu dot bir hücre aşağı gitsin.
            row -= 1;
        }
    }
}
