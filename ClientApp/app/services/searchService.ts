import {inject} from 'aurelia-framework';
import {Config} from 'aurelia-api';
import {Result, IResult} from '../infrastructure/result';

@inject(Config)
export class SearchService {
    constructor(private config: Config) {

    }

    public getResults(resultType: "mentors"|"proteges"): Promise<IResult[]> {
        return this.config.getEndpoint('api').find(resultType);
    }
}