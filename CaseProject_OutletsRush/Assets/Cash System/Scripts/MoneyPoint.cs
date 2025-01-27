using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MoneyPoint : MonoBehaviour
{
    [Range(0, 20)] [SerializeField] private float upwardSpeed;

    [SerializeField] private Color _color;
    [SerializeField] private Vector2 randomSpeed;
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private int pointValue;

    private float simulationSpeed;
    public void AnimationStart(int moneyAmount)
    {
        transform.localPosition = Vector3.zero;
        simulationSpeed = Random.Range(randomSpeed.x, randomSpeed.y);
        pointText.color = _color;
        pointText.text = "$" + pointValue.ToString();
        StopAllCoroutines();
        StartCoroutine(SetMoneyVal(moneyAmount));
        StartCoroutine(PointUp());
    }
    IEnumerator PointUp()
    {

        float counter = 0;
        float currentSpeed = 0;
        while (counter < Mathf.PI / 2)
        {
            counter += simulationSpeed * Time.deltaTime;
            currentSpeed = Mathf.Cos(counter);
            currentSpeed *= upwardSpeed;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 1, 0), Time.deltaTime * currentSpeed);

            yield return null;
        }
    }

  

    IEnumerator SetMoneyVal(int moneyAmount)
    {
        float counter = 0f;
        int decimalCounter = 0;

        while (counter < 1f)
        {
            decimalCounter++;
            counter += Time.deltaTime;
            float money = Mathf.Lerp(0 , (float)moneyAmount, counter);
            pointText.text = "$" + CoefficientTransformation.Converter((int)money);
       
            yield return null;
        }

        pointText.text = "$" + CoefficientTransformation.Converter((int)moneyAmount);

        yield return new WaitForSeconds(1f);

        StartCoroutine(ColorSet());

    }
    IEnumerator ColorSet()
    {
        float counter = 0;
        while (counter < Mathf.PI / 2)
        {
            counter += 4 * simulationSpeed * Time.deltaTime;
            float currentAlpha = (counter / (Mathf.PI / 2));
            pointText.color = new Color(pointText.color.r, pointText.color.g, pointText.color.b, Mathf.Abs(1 - currentAlpha));
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
