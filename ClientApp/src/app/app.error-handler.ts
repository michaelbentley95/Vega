import * as Sentry from '@sentry/browser';
import { ToastaService } from 'ngx-toasta';
import { ErrorHandler, NgZone, isDevMode } from "@angular/core";

export class AppErrorHandler implements ErrorHandler {
    constructor(
        private ngZone: NgZone,    
        private toastaService: ToastaService
    ){}
    
    handleError(error: any): void {
        if(!isDevMode()){
            Sentry.captureException(error.originalError || error);
        }

        this.ngZone.run(() =>{
            this.toastaService.error({
                title: "Error",
                msg: "An unexpected error happened.",
                showClose: true,
                timeout: 5000,
                theme: "bootstrap"
            });
        })
    }
}