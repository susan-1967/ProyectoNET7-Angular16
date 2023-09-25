import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { IResponse } from '../../../../commons/services/api/api-models-base.interface';
import { environment } from '../../../../../environments/environment';
import { IRequestCreateUpdateGenre, IResponseGenre } from './tipo-api-model.interface';

export const URL_GENRE = environment.host + '/tipo';

@Injectable()
export class TipoApiService {
	private _httpClient = inject(HttpClient);

	createTipo(request: IRequestCreateUpdateGenre): Observable<IResponse<number>> {
		return this._httpClient.post<IResponse<number>>(URL_GENRE, request);
	}

	getTipos(): Observable<IResponse<IResponseGenre[]>> {
		return this._httpClient.get<IResponse<IResponseGenre[]>>(URL_GENRE);
	}

	getTipo(id: number): Observable<IResponse<IResponseGenre>> {
		const url = `${URL_GENRE}/${id}`;
		return this._httpClient.get<IResponse<IResponseGenre>>(url);
	}

	updateTipo(id: number, request: Partial<IRequestCreateUpdateGenre>): Observable<IResponse<number>> {
		const url = `${URL_GENRE}/${id}`;
		return this._httpClient.put<IResponse<number>>(url, request);
	}

	deleteTipo(id: number): Observable<IResponse<number>> {
		const url = `${URL_GENRE}/${id}`;
		return this._httpClient.delete<IResponse<number>>(url);
	}
}
