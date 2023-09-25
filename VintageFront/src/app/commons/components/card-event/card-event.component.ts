import { NgClass, NgIf } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ICardEvent } from '../../models/components.interface';

@Component({
	standalone: true,
	selector: 'app-card-event',
	templateUrl: './card-event.component.html',
	styleUrls: ['./card-event.component.scss'],
	imports: [MatCardModule, NgClass, NgIf]
})
export class CardEventComponent {
	@Input() event?: ICardEvent;
	@Output() clickCard = new EventEmitter<ICardEvent>();

	isSelect = false;

	clickEvent(): void {
		this.isSelect = true;
		this.clickCard.emit(this.event);
	}
}
