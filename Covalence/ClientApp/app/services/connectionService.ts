import {inject} from 'aurelia-framework';
import {Config} from 'aurelia-api';

@inject(Config)
export class ConnectionService {
    constructor(private config: Config) {
        
    }
}