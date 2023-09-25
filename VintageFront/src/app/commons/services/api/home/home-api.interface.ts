export interface IResponseHome {
	tipos: IHomeGenres[];
	productos: IHomeConcerts[];
	success: boolean;
}

export interface IHomeConcerts {
	id: number;
	nombre: string;
	description: string;
	unitPrice: number;
	tipo: string;
	tipoId: number;
	dateEvent: string;
	timeEvent: string;
	imageUrl: string;
	cantidad: number;
	finalized: boolean;
	status: string;
}

export interface IHomeGenres {
	id: number;
	name: string;
	status: boolean;
}
