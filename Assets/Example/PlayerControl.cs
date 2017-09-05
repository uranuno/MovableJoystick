using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControl : MonoBehaviour {

	public float speed;
	public float turnSmooth;

	Rigidbody m_rigidbody;
	Transform m_transform;

	Vector2 m_lookTarget;
	
	void Awake () {
		Application.targetFrameRate = 60;
	}

	void Start () {
		m_rigidbody = GetComponent<Rigidbody> ();
		m_transform = transform;
	}

	void Update () {
		// 動く入力
		Vector2 moveInput = new Vector2 (
			CrossPlatformInputManager.GetAxisRaw ("Horizontal"),
			CrossPlatformInputManager.GetAxisRaw ("Vertical")
		);

		// 動く方向をみる
		m_lookTarget = moveInput;

		// 動く方向があるとき
		if (m_lookTarget != Vector2.zero) {
			// 向きを更新
			m_transform.rotation = Quaternion.Lerp(
				m_transform.rotation,
				Quaternion.LookRotation (new Vector3 (m_lookTarget.x, 0, m_lookTarget.y)),
				turnSmooth * Time.deltaTime
			);
		}
	}

	void FixedUpdate () {
		// 動く方向があるときだけ進む
		if (m_lookTarget != Vector2.zero) {
			Vector3 movement = new Vector3(m_lookTarget.x, 0, m_lookTarget.y);
			m_rigidbody.velocity = new Vector3 (
				movement.x * speed,
				m_rigidbody.velocity.y,//Y軸は速度をいじらない
				movement.z * speed
			);
		}
	}
}
