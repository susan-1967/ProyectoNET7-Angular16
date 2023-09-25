import { ICardEvent } from '../../../models/components.interface';
import { IHomeConcerts, IHomeGenres, IResponseHome } from './home-api.interface';

export class ResponseHome {
	tipos!: IHomeGenres[];
	productos!: IHomeConcerts[];

	constructor(data: IResponseHome) {
		this.tipos = data.tipos;
		this.productos = data.productos;
	}

	getDataCardEvent(): ICardEvent[] {
		return this.productos.map((item) => {
			const event: ICardEvent = {
				idEvent: item.id,
				urlImage: item.imageUrl,
				nombre: item.nombre,
				description: item.description,
				date: item.dateEvent,
				hour: item.timeEvent,
				price: item.unitPrice,
				tipo: item.tipo
				};

			return event;
		});
	}
}
