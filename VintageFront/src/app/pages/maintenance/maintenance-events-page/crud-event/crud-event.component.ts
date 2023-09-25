import { Component, OnInit, inject } from '@angular/core';
import { CRUD_METHOD } from '../../../../commons/models/enums';
import { IResponseGenre } from '../../services/tipo/tipo-api-model.interface';
import { TipoApiService } from '../../services/service-index';
import { EventsService } from '../events.service';

@Component({
	selector: 'app-crud-event',
	templateUrl: './crud-event.component.html',
	styleUrls: ['./crud-event.component.scss']
})
export class CrudEventComponent implements OnInit {
	private _genreApiService = inject(TipoApiService);

	eventsService = inject(EventsService);

	listGenres: IResponseGenre[] = [];

	ngOnInit(): void {
		this._loadGenres();
	}

	private _loadGenres(): void {
		this._genreApiService.getTipos().subscribe((response) => {
			this.listGenres = response.data;
		});
	}
	ClickSave(): void {
		console.log("Grabar");
		this.eventsService.saveEvent();
		
	}

	onFileSelected(event: Event): void {
		const htmlInput: HTMLInputElement = event.target as HTMLInputElement;
		if (htmlInput && htmlInput.files && htmlInput.files.length > 0) {
			const reader = new FileReader();
			reader.readAsDataURL(htmlInput.files[0]);
			reader.onload = () => {
				const resultImageFile = reader.result!.toString();
				this.eventsService.fileNameField.setValue(htmlInput.files![0].name);
				this.eventsService.imageField.setValue(resultImageFile);
			};
		}
	}
	

	clickClear(): void {
		this.eventsService.crudMethod = CRUD_METHOD.SAVE;
		this.eventsService.eventsFormGroup.reset();
	}
	Save(): void {
		console.log("Grabar");
		this.eventsService.saveEvent();
		
	}
}
