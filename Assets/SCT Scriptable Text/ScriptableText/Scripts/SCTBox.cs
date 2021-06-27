using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace SCT
{
	[ExecuteAlways]
	public class SCTBox : MonoBehaviour
	{
		[SerializeField] private Image m_leftIcon;
		[SerializeField] private Image m_rightIcon;
		[SerializeField] private Image m_background;
		[SerializeField] private TextMeshProUGUI m_textMesh;

		private RectTransform m_rectTransform;
		private Vector2 m_previousSizeDelta = new Vector2(0, 0);
		private Vector2 m_sizeDelta = new Vector2(0, 0);
		private Canvas m_canvas;

		private void Start()
		{
			m_rectTransform = GetComponent<RectTransform>();
			m_canvas = ScriptableTextDisplay.Instance.gameObject.GetComponent<Canvas>();
		}

		private void Update()
		{
			UpdateAnchor();
			UpdateSizeBasedOnContent();
			UpdateTextPosition();
		}

		private void UpdateSizeBasedOnContent()
		{
			m_sizeDelta.x = 0;
			m_sizeDelta.y = 0;

			//width
			m_sizeDelta.x +=
				m_leftIcon.isActiveAndEnabled ? m_leftIcon.rectTransform.rect.width : 0;
			m_sizeDelta.x += m_rightIcon.isActiveAndEnabled
				? m_rightIcon.rectTransform.rect.width
				: 0;

			var textWidth = m_textMesh.rectTransform.rect.width;
			var backgroundWidth =
				m_background.isActiveAndEnabled ? m_background.rectTransform.rect.width : 0;
			m_sizeDelta.x += textWidth > backgroundWidth ? textWidth : backgroundWidth;

			//height
			var leftIconHeight =
				m_leftIcon.isActiveAndEnabled ? m_leftIcon.rectTransform.rect.height : 0;
			var rightIconHeight =
				m_rightIcon.isActiveAndEnabled ? m_rightIcon.rectTransform.rect.height : 0;
			var textHeight = m_textMesh.rectTransform.rect.height;
			var backgroundHeight =
				m_background.isActiveAndEnabled ? m_background.rectTransform.rect.height : 0;

			var centerMaxHeight = textHeight > backgroundHeight ? textHeight : backgroundHeight;
			var iconMaxHeight = leftIconHeight > rightIconHeight ? leftIconHeight : rightIconHeight;

			var maxHeight = centerMaxHeight > iconMaxHeight ? centerMaxHeight : iconMaxHeight;
			m_sizeDelta.y = maxHeight;

			if (m_previousSizeDelta != m_sizeDelta)
			{
				m_rectTransform.sizeDelta = m_sizeDelta;
				m_previousSizeDelta = m_sizeDelta;
			}
		}

		private void UpdateAnchor()
		{
			var anchorLeft = m_rightIcon.isActiveAndEnabled;
			var anchorRight = m_leftIcon.isActiveAndEnabled;

			if (anchorLeft)
			{
				SetTextAnchor(new Vector2(0, 0.5f),
							  new Vector2(0, 0.5f),
							  new Vector2(0, 0.5f));
			}

			if (anchorRight)
			{
				SetTextAnchor(new Vector2(1, 0.5f),
							  new Vector2(1, 0.5f),
							  new Vector2(1, 0.5f));
			}

			if (!anchorLeft && !anchorRight)
			{
				SetTextAnchor(new Vector2(0.5f, 0.5f),
							  new Vector2(0.5f, 0.5f),
							  new Vector2(0.5f, 0.5f));
			}

			if (anchorLeft && anchorRight)
			{
				SetTextAnchor(new Vector2(0.5f, 0.5f),
							  new Vector2(0.5f, 0.5f),
							  new Vector2(0.5f, 0.5f));
			}
		}

		private void SetTextAnchor(Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot)
		{
			m_textMesh.rectTransform.anchorMin = anchorMin;
			m_textMesh.rectTransform.anchorMax = anchorMax;
			m_textMesh.rectTransform.pivot = pivot;
		}

		private void UpdateTextPosition()
		{
			var anchorLeft = m_rightIcon.isActiveAndEnabled;
			var anchorRight = m_leftIcon.isActiveAndEnabled;

			if (anchorLeft && anchorRight)
			{
				var textWidth = m_textMesh.rectTransform.rect.width;
				var rightOffset = m_rightIcon.rectTransform.rect.width;
				var leftOffset = m_leftIcon.rectTransform.rect.width;

				var centerOffset = (leftOffset - rightOffset) / 2;
				m_textMesh.transform.localPosition = new Vector3(centerOffset, 0);
			}
		}
	}
}
#pragma warning restore 0649