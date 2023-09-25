//#region  CREATE  / UPDATE EVENT
export interface IRequestCreateUpdateConcert {
	dateEvent: string;
	nombre: string;
	description: string;
	timeEvent: string;
	idTipo: number;
	place: string;
	base64Image: string;
	fileName: string;
	unitPrice: number;
	cantidad: number;
}

//#endregion

//#region GET LIST CONCERTS
export interface IResponseConcert {
	id: number;
	nombre: string;
	dateEvent: string;
	timeEvent: string;
	tipo: string;
	tipoId: number;
	imageUrl: string;
	description: string;
	cantidad: number;
	unitPrice: number;
	status: string;
}

interface IGenre {
	id: number;
	name: string;
}
//#endregion
