using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControl : MonoBehaviour {

	public float speed;
	public float turnSmooth;

	new Rigidbody rigidbody;
	new Transform transform;

	Vector2 m_lookTarget;
	
	void Awake () {
		Application.targetFrameRate = 60;
	}

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		transform = base.transform;
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
			transform.rotation = Quaternion.Lerp(
				transform.rotation,
				Quaternion.LookRotation (new Vector3 (m_lookTarget.x, 0, m_lookTarget.y)),
				turnSmooth * Time.deltaTime
			);
		}
	}

	void FixedUpdate () {
		// 動く方向があるときだけ進む
		if (m_lookTarget != Vector2.zero) {
			Vector3 movement = new Vector3(m_lookTarget.x, 0, m_lookTarget.y);
			rigidbody.velocity = new Vector3 (
				movement.x * speed,
				rigidbody.velocity.y,//Y軸は速度をいじらない
				movement.z * speed
			);
		}
	}
}
