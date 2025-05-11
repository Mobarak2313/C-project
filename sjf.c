#include <stdio.h>

int main(){

    int n,p[20],bt[20],wt[20],tat[20],i;
    double avwt=0, avtat=0;


    printf("Enter Process number : ");
    scanf("%d",&n);

    printf("\nEnter Burst Time : \n");

    for(i=0; i<n; i++){
        printf("P[%d] : ",i+1);
        scanf("%d",&bt[i]);
        p[i]=i+1;
    }


    /////////////////// sorting ////////////////
    int minI,temp,j;
    for(i=0; i<n-1; i++){

        minI=i;

        for(j=i+1;j<n;j++){
            if(bt[j]<bt[minI]){
                minI=j;
            }
        }

        temp=bt[i];
        bt[i]=bt[minI];
        bt[minI]=temp;

        temp=p[i];
        p[i]=p[minI];
        p[minI]=temp;



    }


    wt[0]=0;

    for(i=1;i<n;i++){
            wt[i]=0;
        for(int j=0; j<i;j++){
            wt[i]+=bt[j];

        }
    }



    for(i=0;i<n;i++){
        tat[i]=wt[i]+bt[i];
        avwt +=wt[i];
        avtat +=tat[i];
    }


    printf("\n\nProcess\tBurst Time\tWaiting Time\tTurnaround Time\n");

    for(i=0;i<n;i++){
       printf("\nP[%d]\t%d\t\t%d\t\t%d\n",p[i],bt[i],wt[i],tat[i]);
    }




    avwt /=n;
    avtat /=n;

    printf("\nAvarage waiting Time : %.2f\n",avwt);
    printf("\nAvarage turnaround Time : %.2f\n",avtat);





    return 0;

}
