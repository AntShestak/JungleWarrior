using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    public static ObjectPicker Instance { get; private set; }

	[SerializeField] WeaponSelect m_weaponSelect;
	[SerializeField] GameObject[] m_pickableWeapons;
	
	

    void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

	public void PickUpItem()
	{
		GameObject item = PickableObjectScanner.Instance.CurrentPickableObject;
		if (item == null)
		{
			Debug.Log("No object to pick up.");
			return;
		}
			
		IPickable.PickableType type = item.GetComponent<IPickable>().Type;
		if (type == IPickable.PickableType.Weapon)
		{
			if (m_weaponSelect == null)
			{
				Debug.Log("No weapon select found!");
				return;
			}

			//var drop = new Dictionary<string, int>();
			Dictionary<string, int> drop = m_weaponSelect.SwapCurrentWeapon(item.gameObject.GetComponent<PickableWeapon>());
			Destroy(item);

			IEnumerator cor = DropWeapon(drop["index"], drop["ammo"]);
			StartCoroutine(cor);
			
			
		}
	}

	IEnumerator DropWeapon(int index, int ammo)
	{
		WaitForSeconds wait = new WaitForSeconds(0.75f);
		yield return wait;
		//instatiate new pickable item
		GameObject obj = Instantiate(m_pickableWeapons[index], transform.position, Quaternion.identity);
		obj.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * 3.5f + transform.up * 0.5f + transform.right * -0.5f, ForceMode.Impulse);
		//yield return wait;
		obj.GetComponentInChildren<PickableWeapon>().Ammo = ammo;
		
	}
}
