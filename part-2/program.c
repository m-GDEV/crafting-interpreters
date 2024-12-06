#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <stdbool.h>

char * strdup2(char * str) {
    char * s = (char *)malloc(sizeof(str));
    strcpy(s, str);
    return s;
}

typedef struct Node Node;
struct Node {
    Node * next;
    Node * prev;
    char * str;
};

Node * create_node(char * str) {
    Node * node = (Node *)malloc(sizeof(Node));
    char * s2 = strdup2(str);
    node->str = s2;
    node->next = NULL;
    node->prev = NULL;

    return node;
}

Node * add_node(Node * head, Node * node) {
    if (head == NULL) {
        return node;
    }

    node->next = head;
    head->prev = node;

    return node;
}

void print_node(Node * node) {
    if (node == NULL) {
        printf("Node is NULL\n");
    }
    else {
        printf("Node: %s\tPrev: %p\tNext: %p\n", node->str, node->prev, node->next);
    }
}

void print_list(Node * head) {
    Node * start = head;

    while(start != NULL) {
        print_node(start);
        start = start->next;
    }
}

Node * find_node(Node * head, char * str) {
    Node * start = head;

    while(start != NULL) {
        int res = strcmp(str, start->str);
        if (res == 0) {
            return start;
        }
        start = start->next;
    }

    return NULL;
}

bool delete_node(Node * head, Node* node) {
    Node * start = head;

    while(start != NULL) {
        int res = strcmp(node->str, start->str);
        if (res == 0) {
            if (start->prev != NULL) {
                start->prev->next = start->next;
            }
            if (start->next != NULL) {
                start->next->prev = start->prev;

            }
            return true;
        }
        start = start->next;
    }

    return false;
}

int main(void) {

    Node * head = create_node("Musa");

    head = add_node(head, create_node("MusaIsCool"));
    head = add_node(head, create_node("MusaIsntCool"));


    print_list(head);

    Node * found_node = find_node(head, "MusaIsCool");
    printf("\nFound Node:\n");
    print_node(found_node);

    Node * found_node2 = find_node(head, "nothere!");
    printf("\nFound Node2:\n");
    print_node(found_node2);

    bool del = delete_node(head, found_node); 
    printf("Deleted node: %s\n", del == true ? "true" : "false"); 

    print_list(head);
    
    return 0;
}
