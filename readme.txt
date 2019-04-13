Questo programma è un divertimento personale. 
Lo scopo è andare sul mio NAS, e mostrarmi le mie foto in ordine casuale mentre io 
continuo ad usare il computer.

Per ora il programma è in uno stato embrionale.
Quando parte viene creato un file di configurazione in formato json.
Non essendoci un path valida per la directory da scansionare il programma va in crash.
Per ora bisogna editare a mano il file di configurazione mettendogli almeno un path con
delle immagini in formato jpg. Per ora vendogono visualizzate solo le immagini in formato
jpg.

Alla partenza viene visualizzata una form ridimensionabile e spostabile.
Con singolo click del tasto sinistro del mouse si forza la transizione alla prossima foto
Facendo doppio click con il tasto sinistro bordi e caption spariscono e la
finestra finisce sullo sfondo, e da lì non si muove.
Con singolo click del tasto destro del mouse si apre una finestra di debug con la posizione
della finestra nello schermo

La durata della transizioni tra due immagini, la fluidità e ogni quanto
ci deve essere una transizione è scritta nel file di configurazione.
I parametri di default sono molto aggressivi.

Cose da fare:

	1 supporto al protocollo sftp
	2 sistema di configurazione interativo
	3 supporto maggiore ai metadati delle immagini
	4 configurazione più ampia (ricorsione cartelle, filtri sulle immagini, etc)
	5 se ci sono molte carte da scansionare la partenza del programma è rallentata
	  la scansione va spostata in un thread separato
	6 il ridimensionamento della finestra durante una transizione fa schifo

Davide Quack