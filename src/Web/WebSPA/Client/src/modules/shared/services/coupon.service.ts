import { Injectable } from '@angular/core';
import { DataService } from '../services/data.service';
import { ConfigurationService } from '../services/configuration.service';
import { ICoupon } from '../models/coupon.model'

import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class CouponService {

    private couponUrl: string = '';

    constructor(private service: DataService, private configurationService: ConfigurationService) {
        if (this.configurationService.isReady)
            this.couponUrl = this.configurationService.serverSettings.purchaseUrl;
        else
            this.configurationService.settingsLoaded$.subscribe(x => this.couponUrl = this.configurationService.serverSettings.purchaseUrl);
    }

    checkValidationCoupon(code: string): Observable<ICoupon> {
        let url = this.couponUrl + `/co/api/v1/coupon/${code}`;

        return this.service.get(url).pipe<ICoupon>(tap((response: any) => {
            return response;
        }));
    }

    getCustomerLoyaltyStatus(): Observable<any> {
        let url = this.couponUrl + `/co/api/v1/customer`;

        return this.service.get(url).pipe<any>(tap((response: any) => {
            console.log(response);
            console.log("Customer");
            debugger;
            return response;
        }));
    }
}
