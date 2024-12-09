using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : SingleTon<QuestManager>
{




}



//npc가 퀘스트 번호리스트를 가지고 있고
//npc가 다양한 퀘스트를 가질 수 있게만들어야할듯?
//퀘스트는 클리어 조건을 가지고 있어야함 함수포인터로 만들어주면 되지않을까?
//아니면 각각 퀘스트를 스크립트로 만들어야하는데 조금 애매
//클리어하면 연계되는 퀘스트가 있을 수 있으므로 이것도 체크해 주어야함
//