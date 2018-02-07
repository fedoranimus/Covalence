import { IPost, PostType } from 'infrastructure/post';
import { autoinject } from 'aurelia-framework';
import { Config } from 'aurelia-api';
import { HttpClient } from 'aurelia-fetch-client';

@autoinject
export class GoogleMapsService {
    private apiKey: string = 'AIzaSyDUNEXcb_7SHw-EsOU-sDciQ9kzJX7UyLQ';

    constructor(private http: HttpClient) {

    }

    async getLocation(zipCode: string): Promise<any> {
        return await this.http.fetch(`https://maps.googleapis.com/maps/api/geocode/json?address=${zipCode}&key=${this.apiKey}`);
    }
}