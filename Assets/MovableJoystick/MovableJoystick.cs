using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MovableJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

	[SerializeField]
	RectTransform stickObject;

	[SerializeField]
	RectTransform baseObject;

	public float radius = 100f;

	public string horizontalAxisName = "Horizontal";
	public string verticalAxisName = "Vertical";

	CrossPlatformInputManager.VirtualAxis horizontalVirtualAxis;
	CrossPlatformInputManager.VirtualAxis verticalVirtualAxis;

	Vector2 startPos;

	void OnEnable () {
		horizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
		verticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);

		CrossPlatformInputManager.RegisterVirtualAxis(horizontalVirtualAxis);
		CrossPlatformInputManager.RegisterVirtualAxis(verticalVirtualAxis);

		// 可動範囲 = 基盤の大きさ
		if (baseObject != null) baseObject.sizeDelta = Vector2.one * radius * 2;

		// 一旦(0,0)へ
		MoveTo(Vector2.zero);
	}

	public void OnPointerDown (PointerEventData data) {
		MoveTo (data.position);
		OnDrag (data);
	}

	void MoveTo (Vector2 pos) {
		startPos = pos;
		stickObject.position = pos;
		if (baseObject != null) baseObject.position = pos;
	}

	public void OnPointerUp (PointerEventData data) {
		stickObject.position = startPos;
		UpdateVirtualAxes (Vector2.zero);
	}

	public void OnDrag (PointerEventData data) {
		Vector2 value = data.position - startPos;
		UpdateVirtualAxes (value);
	}

	void UpdateVirtualAxes (Vector2 value) {
		// 移動位置を指定の半径内で制限する
		value = Vector2.ClampMagnitude (value, radius);

		stickObject.position = startPos + value;

		Vector2 normalizedValue = value / radius;
		horizontalVirtualAxis.Update (normalizedValue.x);
		verticalVirtualAxis.Update (normalizedValue.y);
	}

	void OnDisable () {
		if (horizontalVirtualAxis != null) {
			horizontalVirtualAxis.Remove ();
		}
		if (verticalVirtualAxis != null) {
			verticalVirtualAxis.Remove ();
		}
	}
}
