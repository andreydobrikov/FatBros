using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Variables

    public float speed = 10f;

    #endregion

    #region Properties

  

    #endregion

    #region Unity functions
   

    private void FixedUpdate()
    {
        MoveBullet();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "border")
            DestroyBullet();
        else if (other.tag == "enemy")
        {
            AttackTheEnemy(other);
            DestroyBullet();
        }
    }

    #endregion

    #region Bullet functions

    private void MoveBullet()
    {
        //if (GameController.Instance.CurrentGameState != GameStates.Pause)
            this.transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
    }

    private void AttackTheEnemy(Collider other)
    {
        //Alien a = other.GetComponent<Alien>();
        //if (a == null)
        //    Debug.LogError("'Bullet' script can't get 'Alien' component!");
        //else
        //{
        //    AlienLoader al = a as AlienLoader;
        //    if (al != null)
        //        al.CountOfBullets = BulletRank;
        //    a.GetDamage();
        //}
    }

    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }

    #endregion
}