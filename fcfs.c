#include <stdio.h>

int main (){

    int n, bt[20], wt[20], tat[20], i;
    double avwt=0,avtat=0;

    printf("Enter process number maximum 20 : ");
    scanf("%d",&n);

    printf("Enter process Burst time \n");

    for(i=0; i<n; i++){
        printf("P[%d] : ",i+1);
        scanf("%d",&bt[i]);
    }

    wt[0]=0;

    for(i=1;i<n;i++){
        wt[i]=0;
        for(int j=0; j<i; j++){
            wt[i] +=bt[j];
        }
    }

    printf("\n\nProcess\t\tBurst time\tWaiting time\tTurnaround time");


    for(i=0;i<n;i++){
        tat[i]=bt[i]+wt[i];
        avwt +=wt[i];
        avtat +=tat[i];
        printf("\nP[%d]\t\t%d\t\t%d\t\t%d",i+1,bt[i],wt[i],tat[i]);
    }


    avwt/=n;
    avtat/=n;

    printf("\nAvarage waiting time : %.2f",avwt);
    printf("\nAvarage turnaround time : %.2f",avtat);



    return 0;


}
