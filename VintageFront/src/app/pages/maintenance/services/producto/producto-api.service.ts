import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { IResponse } from '../../../../commons/services/api/api-models-base.interface';
import { IRequestCreateUpdateConcert, IResponseConcert } from './producto-api-model.interface';

const URL_CONCERT = environment.host + '/Producto';

@Injectable()
export class ProductoApiService {
	private _httpClient = inject(HttpClient);

	createProducto(event: IRequestCreateUpdateConcert): Observable<IResponse<number>> {
		return this._httpClient.post<IResponse<number>>(URL_CONCERT, event);
	}

	updateProducto(idEvent: number, event: IRequestCreateUpdateConcert): Observable<IResponse<number>> {
		const url = `${URL_CONCERT}/${idEvent}`;
		return this._httpClient.put<IResponse<number>>(url, event);
	}

	deleteProducto(idEvent: number): Observable<IResponse<number>> {
		const url = `${URL_CONCERT}/${idEvent}`;
		return this._httpClient.delete<IResponse<number>>(url);
	}

	getListProductos(page?: number, rows?: number, filter?: string): Observable<IResponse<IResponseConcert[]>> {
		let params = new HttpParams();
		if (filter) {
			params = params.set('filter', filter);
		}

		if (page) {
			params = params.set('page', page);
		}

		if (rows) {
			params = params.set('pageSize', rows);
		}

		return this._httpClient.get<IResponse<IResponseConcert[]>>(URL_CONCERT, { params });
	}

	getProducto(id: number): Observable<IResponse<IResponseConcert>> {
		const url = `${URL_CONCERT}/${id}`;
		return this._httpClient.get<IResponse<IResponseConcert>>(url);
	}

	finalizeProducto(idConcert: number): Observable<IResponse> {
		const url = `${URL_CONCERT}/${idConcert}`;
		return this._httpClient.patch<IResponse>(url, {});
	}
}
