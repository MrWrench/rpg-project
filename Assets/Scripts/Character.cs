using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
  public PersistentStats stats;

  #region Stats

  public float health;
  public float poise;

  #endregion
  

  // Start is called before the first frame update
  void Start()
  {
    health = stats.maxHealth;
    poise = stats.maxPoise;
  }

  // Update is called once per frame
  void Update()
  {
  }
}