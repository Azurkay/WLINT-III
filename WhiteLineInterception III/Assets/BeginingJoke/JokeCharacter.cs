using System.Xml.Serialization;
using UnityEngine;

public class JokeCharacter : MonoBehaviour
{
    [SerializeField] private RectTransform _pos1;
    [SerializeField] private RectTransform _pos2;

    [SerializeField] private Canvas _weaponParent;
    [SerializeField] private RectTransform _throwPosition;
    [SerializeField] private JokeWeapon _weapon;

    private void ThrowWeapon()
    {
        Instantiate(_weapon, _throwPosition.position, Quaternion.identity, _weaponParent.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position = _pos1.position;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position = _pos2.position;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ThrowWeapon();
        }
    }
}
