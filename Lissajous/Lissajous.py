# -*- coding: utf-8 -*-
"""
Created on Wed May 20 12:26:47 2020

@author: frang
"""
import random
import csv
import numpy
import math
import copy
import time


def lireFichier(nomfichier):
    with open(nomfichier,'r') as fichier:
        lines=csv.reader(fichier,delimiter=';')
        fichier=[x for x in lines]#avec premier element
        liste_csv=[]
        for i in range(1,len(fichier)): ## enleve le premier element car le fichier contient dans la premiere ligne: #t,x,y ....
            liste_csv.append(fichier[i])
        return liste_csv
        
        
                    
#print("\n\nSample List:\n",lireFichier('sample.csv'))



class individu:
    
    def __init__(self, length=6):
         self.liste=[round(random.uniform(-10,10),3) for x in range(length)]
      
    def __str__(self):
       return ";".join([str(x) for x in self.liste])
    
    def __repr__(self):
        return self.__str__()
        
    def __getitem__(self,ind):
        return self.liste[ind]
        
    def __setitem__(self,ind,val):
        assert ind in range(6)
        if(ind==0):
           self.liste[ind]=val
        elif(ind==1):
           self.liste[ind]=val
        elif (ind==2):
           self.liste[ind]=val
        elif (ind==3):
           self.liste[ind]=val
        elif (ind==4):
           self.liste[ind]=val
        elif (ind==5):
           self.liste[ind]=val
        
    def fitness(self):
        #On peut modifier le fichier sample.csv par sampleA.csv ou sampleB.csv
        liste_sample=lireFichier('sample.csv')
        liste_moy=[]
        new_list=[]
        liste_fitx=[]
        for j in range(len(liste_sample)):
                new_list.append([round(self.liste[0]*math.sin(self.liste[1]*float(liste_sample[j][0])+self.liste[2]),3),round(self.liste[3]*math.sin(self.liste[4]*float(liste_sample[j][0])+self.liste[5]),3)])  
                liste_fitx.append((abs(new_list[j][0]-float(liste_sample[j][1]))+abs(new_list[0][1]-float(liste_sample[j][2])))/2)
        liste_moy.append(round(numpy.mean(liste_fitx),3))
        return liste_moy
    
             
def create_rand_pop(count):
    return[individu() for _ in range(count)]
    

#evaluation suivant la fitness
#on peut zipper la liste d'individu et celle de fitness puis trier suivant un la fitness et on en ressort deux listes triées comme souhaitée
def evaluate_pop(popu):
    return sorted(popu, key=lambda x:x.fitness())
    
#Je selectionne les meilleurs, ie ceux qui ont la meilleure fitness
# le reste est telle que je ne prends certains individus peuvant avoir un bon paramètre 
    
def selection(pop,number):
    return pop[:number]
    
  
def crossover(parent1,parent2):
    a=individu()
    alpha=0.6  
    indice=round(random.randint(0,5),3)
    a=copy.deepcopy(parent1)
    a[indice]=round(alpha*parent1[indice]+ (1-alpha)*parent2[indice],3)
    return a
        

def mutation(enfant):
        pm=0.25
        if pm>random.uniform(0,1):
            indice=random.randint(0,5)
            nouvelle_valeur=round(random.uniform(-10,10),3)
            enfant[indice]=nouvelle_valeur     
        return enfant
        
    
def iteration():
    pop_create=create_rand_pop(100)
    pop=copy.deepcopy(pop_create)
    solutiontrouvee=False
    nbiter=0
    while not solutiontrouvee:
        print("Iteration:",nbiter)
        nbiter+=1
        evaluation=evaluate_pop(pop)
        if evaluation[0].fitness()[0]<1.5:
            solutiontrouvee=True
            print("_______________________________________________________________________________")
            print("\nAvec ces paramètres: [",evaluation[0],"] nous allons réduire ce satellite en poussière.","\n","\nItération:",nbiter)

        else:
            select=selection(evaluation,2)
            croise=[]
            croise.append(crossover(select[0],select[1]))
            croise.append(crossover(select[1],select[0]))
            #les deux # ci-dessous permettent de voir l'evolution a chaque iteration
            #print("\nLe croisement:\n",croise)
            newalea=[]
            for i in range(len(croise)):
                a=copy.deepcopy(mutation(croise[i]))
                newalea.append(a)
            #print("\nLa mutation:\n",newalea)
            for i in range(2,len(evaluation)):
                newalea.append(evaluation[i])
        pop=copy.deepcopy(newalea)
        p=evaluate_pop(newalea)
        print("\nFitness meilleur individu:",p[0].fitness())
                
         
    
def fonction_test():
    
    one_indiv=individu()
    pop_create=create_rand_pop(10)
    pop_test=create_rand_pop(10)   


    print("\ntest fitness\n",one_indiv.fitness())
    print("\ntest pop\n",pop_create[0])
    print("\ntest getitem",pop_create[0].__getitem__(2))
    print("\ntest setitem",pop_create[0].__setitem__(2,4566),"\n4566 appear\n",pop_create)
    print("\n\n")
    print("\ntest create Population: \n",pop_test)
    print("\n")
    print("test fitness on population")
    for i in range(len(pop_create)):
        print(pop_test[i].fitness(),end="")
    print("\n")


    print("\test evaluate Population: \n",evaluate_pop(pop_test))
    print("\ntest selection in the population:\n",selection(pop_test,20))
    a=selection(pop_test,20)
    b=crossover(a[0],a[1])
    c=crossover(a[1],a[0])
    print("\ntest croisement: \n",a[0],b,a[1],c)
    
    print("\ntest mutation (pm=0.05)\nmutation can't appear: \n",mutation(b),mutation(c))
    
    
if __name__=="__main__":
            
    
#________________________Test des fonctions________________________
    
   #Vous pouvez tester les fonctions en enlevant le # ci-dessous:
   
   #fonction_test()
#__________________________________________________________________
 
    
#_______________________Mise en route du programme_________________
   
   start = time.time()
   iteration()
   print("\nTemps de recherche: %s secondes" % round((time.time() - start),2))
   print("_______________________________________________________________________________")
   
#__________________________________________________________________

