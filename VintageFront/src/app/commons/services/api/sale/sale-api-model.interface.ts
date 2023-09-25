//#region CREATE SALE
export interface IRequestCreateSale {
	productoId: number;
	cantidad: number;
}

//#endregion

//#region ListSalesByGenre
export interface IRequestListSalesByGenre {
	dateStart: string;
	dateEnd: string;
	page?: number;
	rows?: number;
}
//#endregion

//#region ListSales
export interface IResponseListSales {
	saleId: number;
	customerName: string;
	concertName: string;
	ticketsQuantity: number;
	totalPrice: number;
	saleDate: string;
	concertDate: string;
}
//#endregion

//#region get sale
export interface IResponseSale {
	id: number;
	dateEvent: string;
	genre: string;
	imageUrl: string;
	title: string;
	numeroOperacion: string;
	fullName: string;
	cantidad: number;
	saleDate: string;
	total: number;
}
//#endregion
