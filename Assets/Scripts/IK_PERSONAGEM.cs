using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_PERSONAGEM : MonoBehaviour
{

    public Animator animator;
    public Transform alvo;
    public Transform maoE,maoE_alvo,maoD,ombro,aimPivot;
    public float maoD_peso;

    public Quaternion maoE_rot;
    public IK_INVENTARIO personagemInv;
    //public PERSONAGEM personagem;



    // Start is called before the first frame update
    void Awake()
    {              
        
        ombro = animator.GetBoneTransform(HumanBodyBones.RightShoulder).transform;
        maoD.localPosition = personagemInv.rifle.maoDPos;
        Quaternion rotDireita = Quaternion.Euler(personagemInv.rifle.maoDRot.x,personagemInv.rifle.maoDRot.y,personagemInv.rifle.maoDRot.z);
        maoD.localRotation = rotDireita;

        //maoE_alvo = personagem.riflePrefab.transform.GetChild(2); 

    }

    void Start()
    {
        //alvo = personagem.camC.transform.GetChild(0);
    }

    // Update is called once per frame

    void FixedUpdate()
    {
            
                if(ombro != null)
                {
                    aimPivot.position = ombro.position;                    
                }
            
       

    }

    void Update()
    {
        
            maoE_rot = maoE_alvo.rotation;
            maoE.position = maoE_alvo.position;

            

            //if(personagem.mirando)
            //{
            //    maoD_peso += Time.deltaTime * 2;
            //}
            //else
            //{
            //    maoD_peso -= Time.deltaTime * 2;
            //}
            maoD_peso = Mathf.Clamp(maoD_peso,0,1);
      
        

    }

    void OnAnimatorIK()
    {

        
            aimPivot.LookAt(alvo);

            animator.SetLookAtPosition(alvo.position);
        
            //if(personagem.mirando)
            //{
            //    animator.SetLookAtWeight(1f,0.3f,1f);
            //    animator.SetLookAtPosition(alvo.position);

            //    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,1);
            //    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,1);
            //    animator.SetIKPosition(AvatarIKGoal.LeftHand,maoE.position);
            //    animator.SetIKRotation(AvatarIKGoal.LeftHand,maoE_rot);

            //    animator.SetIKPositionWeight(AvatarIKGoal.RightHand,maoD_peso);
            //    animator.SetIKRotationWeight(AvatarIKGoal.RightHand,maoD_peso);
            //    animator.SetIKPosition(AvatarIKGoal.RightHand,maoD.position);
            //    animator.SetIKRotation(AvatarIKGoal.RightHand,maoD.rotation);
            //}
            //else
            //{
            //    animator.SetLookAtWeight(0.4f,0.4f,0.4f);
            //    animator.SetLookAtPosition(alvo.position);

            //    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,0);
            //    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,0);
            //    animator.SetIKPosition(AvatarIKGoal.LeftHand,maoE.position);
            //    animator.SetIKRotation(AvatarIKGoal.LeftHand,maoE_rot);

            //    animator.SetIKPositionWeight(AvatarIKGoal.RightHand,maoD_peso);
            //    animator.SetIKRotationWeight(AvatarIKGoal.RightHand,maoD_peso);
            //    animator.SetIKPosition(AvatarIKGoal.RightHand,maoD.position);
            //    animator.SetIKRotation(AvatarIKGoal.RightHand,maoD.rotation);

            //}
        
       
   
    }
    

  
}
