
public class UbhExplosion : UbhMonoBehaviour
{
    private void OnAnimationFinish()
    {
        Destroy(gameObject);
    }
}
