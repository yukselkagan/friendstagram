import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { CommonService } from '../services/common.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private commonService: CommonService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError( error => {

        console.log("Error catched "+ error['status']);
        switch(error.status){
          case(0):
            this.commonService.showToast("Can not connect API", "error");
            break;
          case(1):
            //something
            break;
          default:
            //this.commonService.showToast("Unkown error", "error");
            break;

        }

        return throwError(() => error );

      })

    );
  }
}
