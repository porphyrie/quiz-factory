#define _CRT_SECURITY_NO_WARNINGS

#include <iostream>
#include <stdlib.h>
#include <time.h>
#include <conio.h>

using namespace std;

class Node {
public:
    int data;
    Node* left;
    Node* right;
    Node(int d) {
        data = d;
        left = NULL;
        right = NULL;
    }
};

Node* buildtree(int elemCount) {
    elemCount--;
    int d = rand() % 90 + 10;
    Node* root;
    if (elemCount == 0) {
        return NULL;
    }
    root = new Node(d);
    root->left = buildtree(elemCount);
    root->right = buildtree(elemCount);
    return root;
}

void print(Node* root) {
    if (root == NULL) {
        return;
    }
    cout << "(";
    cout << "n:" << root->data;
    if (root->left != NULL) {
        cout << ",";
        cout << "s:" << root->left->data;
    }
    if (root->right != NULL) {
        cout << ",";
        cout << "d:" << root->right->data;
    }
    cout << ")";
    print(root->left);
    print(root->right);
}

void printPreorder(Node* root) {
    if (root == NULL) {
        return;
    }
    cout << root->data << " ";
    printPreorder(root->left);
    printPreorder(root->right);
}

void printInorder(Node* root) {
    if (root == NULL) {
        return;
    }
    printInorder(root->left);
    cout << root->data << " ";
    printInorder(root->right);
}

void printPostorder(Node* root) {
    if (root == NULL) {
        return;
    }
    printPostorder(root->left);
    printPostorder(root->right);
    cout << root->data << " ";
}

int main()
{   
    srand(time(NULL));

    int elemCount = 5;
    Node* root = buildtree(elemCount);

    printf("\n#BEFORE#\n");
    print(root);
    printf("\n#BEFORE#\n");

    printf("\n#AFTER#\n");
    printPreorder(root);
    printf("\n#AFTER#\n");

    return 1;
}