#include <stdio.h>

int main() {
    int n,t=0,qt,i;
    double avwt = 0, avtat = 0;

    printf("Enter process number: ");
    scanf("%d", &n);

    int bt[n], wt[n], tat[n],rm_bt[n];

    printf("\nEnter process Burst Time\n");

    for (i = 0; i < n; i++) {
        printf("P[%d]: ", i + 1);
        scanf("%d", &bt[i]);
        rm_bt[i] = bt[i];
    }

    printf("Enter time quantum: ");
    scanf("%d", &qt);


    while (1) {
        int done = 1;
        for (int i = 0; i < n; i++) {
            if (rm_bt[i] > 0) {
                done = 0;
                if (rm_bt[i] > qt) {
                    t += qt;
                    rm_bt[i] -= qt;
                } else {
                    t += rm_bt[i];
                    wt[i] = t - bt[i];
                    rm_bt[i] = 0;
                }
            }
        }
        if (done) break;
    }


    printf("\nProcess\t\tBurst Time\tWaiting Time\tTurnaround Time");

    for (i = 0; i < n; i++) {
        tat[i] = bt[i] + wt[i];
        avwt += wt[i];
        avtat += tat[i];
        printf("\nP[%d]\t\t%d\t\t%d\t\t%d", i + 1, bt[i], wt[i], tat[i]);
    }

    avwt /= n;
    avtat /= n;

    printf("\n\nAverage Waiting Time: %.2f", avwt);
    printf("\nAverage Turnaround Time: %.2f\n", avtat);

    return 0;
}
