using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MovableJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

	[SerializeField]
	RectTransform content;

	[SerializeField]
	RectTransform stick;

	public float radius = 100f;

	public string horizontalAxisName = "Horizontal";
	public string verticalAxisName = "Vertical";

	CrossPlatformInputManager.VirtualAxis horizontalVirtualAxis;
	CrossPlatformInputManager.VirtualAxis verticalVirtualAxis;

	void OnEnable () {
		horizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
		verticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);

		CrossPlatformInputManager.RegisterVirtualAxis(horizontalVirtualAxis);
		CrossPlatformInputManager.RegisterVirtualAxis(verticalVirtualAxis);

		// 可動範囲 = 基盤の大きさ
		content.sizeDelta = Vector2.one * radius * 2;
		// stickの親がcontent
		stick.SetParent(content,false);

		// 一旦(0,0)へ
		MoveTo(Vector2.zero);
	}

	public void OnPointerDown (PointerEventData data) {
		MoveTo (data.position);
	}

	void MoveTo (Vector2 pos) {
		content.position = pos;
		stick.localPosition = Vector2.zero;
	}

	public void OnPointerUp (PointerEventData data) {
		UpdateVirtualAxes (Vector2.zero);
	}

	public void OnDrag (PointerEventData data) {
		Vector2 value = (Vector2)data.position - (Vector2)content.position;
		UpdateVirtualAxes (value);
	}

	void UpdateVirtualAxes (Vector2 value) {
		// 移動位置を指定の半径内で制限する
		value = Vector2.ClampMagnitude (value, radius);
		// stickを移動
		stick.localPosition = value;

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
