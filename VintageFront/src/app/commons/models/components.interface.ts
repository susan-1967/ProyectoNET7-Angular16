//#endregion CARD-EVENT
export interface ICardEvent {
	idEvent: number;
	urlImage: string;
	nombre: string;
	description: string;
	date: string;
	hour: string;
	price: number;
	tipo: string;
}
//#endregion

//#endregion CARD MENU
export interface ICardMenu {
	title: string;
	nameImage: string;
	path: string;
}
//#endregion
