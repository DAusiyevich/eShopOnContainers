import { ITier } from "./tier.model";

export interface ICustomerLoyalty {
    pointsAvaliable: number;
    maoneySpent: number;
    message: string;
    loyaltyTier: ITier;
}