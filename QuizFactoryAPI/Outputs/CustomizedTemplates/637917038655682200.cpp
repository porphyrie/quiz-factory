#define _CRT_SECURITY_NO_WARNINGS

#include <iostream>
#include <stdlib.h>
#include <time.h>
#include <conio.h>

using namespace std;

struct nod {
	int n;
	nod* prec;
};

nod* vf;

void push(int m) {
	if (vf == NULL) {
		vf = new nod;
		vf->n = m;
		vf->prec = NULL;
	}
	else {
		nod* p = new nod;
		p->n = m;
		p->prec = vf;
		vf = p;
	}
}

void pop() {
	nod* q = vf;
	vf = vf->prec;
	delete q;
}

void list_elements() {
	nod* p = vf;
	while (p) {
		cout << p->n << ' ';
		p = p->prec;
	}
}

nod* searchNod(int m) {
	nod* q = vf;
	while (q != NULL) {
		if (q->n == m)
			return q;
		q = q->prec;
	}
	return NULL;
}

void generate_output() {
    nod *q = vf;
    while (q->prec && (q->prec->n >= q->n / 3)) {
        q = q->prec;
        nod *v = vf;
        vf = vf->prec;
        delete v;
    }
}
int main() {

	srand(time(NULL));

	int elemCount = rand() % 3 + 4;

	// push nodes
	for (int i = 0; i < elemCount; i++) {
		int m = rand() % 10;
		if (!searchNod(m))
			push(m);
		else
			i--;
	} 

	printf("\n#BEFORE#\n");
	list_elements();
	printf("\n#BEFORE#\n");
	
	generate_output();

	printf("\n#AFTER#\n");
	list_elements();
	printf("\n#AFTER#\n");

	return 1;
}






