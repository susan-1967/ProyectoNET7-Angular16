import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { PATHS_AUTH_PAGES, PATH_MY_ACCOUNT_PAGES } from '../../../../config/path-pages';
import { ChannelHeaderService } from '../../../../services/local/channel-header.service';
import { DataUserService } from '../../../../services/local/data-user.service';
import { SessionStorageService } from '../../../../services/local/storage/storage.service';

@Component({
	selector: 'app-header',
	templateUrl: './header.component.html',
	styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
	private _channelService = inject(ChannelHeaderService);
	private _sessionStorageService = inject(SessionStorageService);
	private _router = inject(Router);
	private _dataUserService = inject(DataUserService);

	readonly loginPath = PATHS_AUTH_PAGES.loginPage.withSlash;
	readonly registerPath = PATHS_AUTH_PAGES.registerPage.withSlash;
	readonly myAccountPath = PATH_MY_ACCOUNT_PAGES.withSlash;

	private _channelHeaderService = inject(ChannelHeaderService);

	userName = '';
	isAdmin = false;
	showUser = false;

	ngOnInit(): void {
		this._subscriptionHeaderChannel();
	}
	private _subscriptionHeaderChannel() {
		this._channelHeaderService.channelHeader$.subscribe((show) => {
			console.log('_subscriptionHeaderChannel--> ', show);
			if (show) {
				this._getDataUser();
			}
			this.showUser = show;
		});
		//this._getDataUser();
	}

	private _getDataUser(): void {
		const dataUser = this._dataUserService.getDataUser();
		if (dataUser) {
			this.showUser = true;
			this.userName = dataUser.fullName;
			this.isAdmin = dataUser.isAdmin;
		}
	}

	clickLogout(): void {
		this.showUser = false;
		this._sessionStorageService.clear();
		this._router.navigateByUrl(PATHS_AUTH_PAGES.loginPage.withSlash);
	}
}
