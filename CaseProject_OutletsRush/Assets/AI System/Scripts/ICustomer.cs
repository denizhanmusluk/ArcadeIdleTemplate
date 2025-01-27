using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICustomer
{
    float currentCommentPoint { get; set; }
    void CaseTargetPosSelect();
    void CustomerGoExit(Transform exitPosTR);
}
