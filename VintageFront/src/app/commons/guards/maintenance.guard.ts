import { Injectable, inject } from '@angular/core';
import {
	ActivatedRouteSnapshot,
	CanActivateChild,
	CanActivateChildFn,
	Router,
	RouterStateSnapshot
} from '@angular/router';
import { PATHS_AUTH_PAGES } from '../config/path-pages';
import { DataUserService } from '../services/local/data-user.service';

@Injectable({
	providedIn: 'root'
})
export class MaintenanceGuard implements CanActivateChild {
	private dataUserService = inject(DataUserService);
	private router = inject(Router);

	canActivateChild(_route: ActivatedRouteSnapshot, _state: RouterStateSnapshot): boolean {
		const user = this.dataUserService.getDataUser();
		if (!user || user.isAdmin === false) {
			this.router.navigateByUrl(PATHS_AUTH_PAGES.loginPage.withSlash);
			return false;
		}

		return true;
	}
}

//#region NUEVO ENFOQUE PARA USAR GUARDS
export const NewMaintenanceGuard: CanActivateChildFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
	const dataUserService = inject(DataUserService);
	const router = inject(Router);

	const user = dataUserService.getDataUser();
	if (!user || user.isAdmin === false) {
		router.navigateByUrl(PATHS_AUTH_PAGES.loginPage.withSlash);
		return false;
	}

	return true;
};
//#endregion
