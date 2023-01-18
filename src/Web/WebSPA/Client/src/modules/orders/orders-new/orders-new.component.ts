import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { OrdersService } from '../orders.service';
import { BasketService } from '../../basket/basket.service';
import { IOrder }                                   from '../../shared/models/order.model';
import { BasketWrapperService }                     from '../../shared/services/basket.wrapper.service';

import { FormGroup, FormBuilder, Validators  }      from '@angular/forms';
import { Router }                                   from '@angular/router';
import { ICoupon }                                  from '../../shared/models/coupon.model';
import { DataService } from '../../shared/services/data.service';
import { CouponService } from '../../shared/services/coupon.service';
import { ICustomerLoyalty } from '../../shared/models/customerLoyalty';

@Component({
    selector: 'esh-orders_new .esh-orders_new .mb-5',
    styleUrls: ['./orders-new.component.scss'],
    templateUrl: './orders-new.component.html'
})
export class OrdersNewComponent implements OnInit {
    newOrderForm: FormGroup;  // new order form
    isOrderProcessing: boolean;
    errorReceived: boolean;
    order: IOrder;
    coupon: ICoupon;
    couponValidationMessage: string;
    customerLoyalty: ICustomerLoyalty;
    pointsUsed: number;
    discountValue: number;

    constructor(private orderService: OrdersService, private basketService: BasketService, fb: FormBuilder, private router: Router, private couponService: CouponService) {
        // Obtain user profile information
        this.order = orderService.mapOrderAndIdentityInfoNewOrder();
        this.newOrderForm = fb.group({
            'street': [this.order.street, Validators.required],
            'city': [this.order.city, Validators.required],
            'state': [this.order.state, Validators.required],
            'country': [this.order.country, Validators.required],
            'cardnumber': [this.order.cardnumber, Validators.required],
            'cardholdername': [this.order.cardholdername, Validators.required],
            'expirationdate': [this.order.expiration, Validators.required],
            'securitycode': [this.order.cardsecuritynumber, Validators.required],
        });

        this.pointsUsed = 0;
        this.customerLoyalty = <ICustomerLoyalty>{};
        this.couponService.getCustomerLoyaltyStatus().subscribe(
            customerLoyalty =>
            {
                this.customerLoyalty = customerLoyalty;
                this.discountValue = Number.parseFloat((this.order.total * (this.customerLoyalty.loyaltyTier.discount / 100)).toFixed(2));
                console.log(this.customerLoyalty);
            },
            error => { console.log(error) });
    }

    ngOnInit() {
    }

    checkValidationCoupon(value: string)
    {
        // this.coupon = <ICoupon>{};
        this.couponService.checkValidationCoupon(value).subscribe(
            coupon => { this.coupon = coupon; },
            error => { this.couponValidationMessage = "Coupon is not valid"; });
    }

    applyPaymnetWithPoints(value: any) {
        debugger;
        let val = Number.parseInt(value);
        if (val <= this.customerLoyalty?.pointsAvaliable) {
            this.pointsUsed = val;
        }
    }

    submitForm(value: any) {
        this.order.street = this.newOrderForm.controls['street'].value;
        this.order.city = this.newOrderForm.controls['city'].value;
        this.order.state = this.newOrderForm.controls['state'].value;
        this.order.country = this.newOrderForm.controls['country'].value;
        this.order.cardnumber = this.newOrderForm.controls['cardnumber'].value;
        this.order.cardtypeid = 1;
        this.order.cardholdername = this.newOrderForm.controls['cardholdername'].value;
        this.order.cardexpiration = new Date(20 + this.newOrderForm.controls['expirationdate'].value.split('/')[1], this.newOrderForm.controls['expirationdate'].value.split('/')[0]);
        this.order.cardsecuritynumber = this.newOrderForm.controls['securitycode'].value;
        let basketCheckout = this.basketService.mapBasketInfoCheckout(this.order);
        if (this.coupon) {
            basketCheckout.couponCode = this.coupon.code;
            basketCheckout.couponValue = this.coupon.discount;
        }
        basketCheckout.pointsUsed = this.pointsUsed;
        basketCheckout.discount = this.customerLoyalty?.loyaltyTier.discount;
        this.basketService.setBasketCheckout(basketCheckout)
            .pipe(catchError((errMessage) => {
                this.errorReceived = true;
                this.isOrderProcessing = false;
                return Observable.throw(errMessage); 
            }))
            .subscribe(res => {
                this.router.navigate(['orders']);
            });
        this.errorReceived = false;
        this.isOrderProcessing = true;
    }
}

