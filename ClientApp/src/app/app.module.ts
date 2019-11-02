import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { TreeModule } from 'angular-tree-component';
import { NgxBladeModule } from 'ngx-blade';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';

import { LoaderComponent } from './loader/loader.component'
import { ConnectionComponent } from './connection/connection.component'

import { CategoryComponent } from './eshop/blades/category/category.component'
import { CategoryBodyComponent } from './eshop/blades/category/category-body.component'

import { BladeHeaderComponent } from './eshop/blades/blade-header.component'
import { GoodListComponent } from './eshop/blades/goodlist/good-list.component'
import { GoodListConfirmDeleteModalContent } from './eshop/blades/goodlist/good-list-confirmdeletemodal.component'
import { GoodListBodyComponent } from './eshop/blades/goodlist/good-list-body.component'

import { EditGoodComponent } from './eshop/blades/goodedit/good-edit.component'
import { EditGoodBodyComponent } from './eshop/blades/goodedit/good-edit-body.component'
import { EditGoodResetModalContent } from './eshop/blades/goodedit/good-edit-resetmodal.component'


import { LoaderService } from './services/loader.service'
import { LoaderInterceptor} from './interceptors/loader.interceptor'

import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { MatProgressBarModule } from '@angular/material/progress-bar'
import { NotifierModule, NotifierOptions } from 'angular-notifier';


/**
 * Custom angular notifier options
 */
const customNotifierOptions: NotifierOptions = {
    position: {
        horizontal: {
            position: 'right',
            distance: 12
        },
        vertical: {
            position: 'top',
            distance: 12,
            gap: 10
        }
    },
    theme: 'material',
    behaviour: {
        autoHide: 5000,
        onClick: 'hide',
        onMouseover: 'pauseAutoHide',
        showDismissButton: true,
        stacking: 4
    },
    animations: {
        enabled: true,
        show: {
            preset: 'slide',
            speed: 300,
            easing: 'ease'
        },
        hide: {
            preset: 'fade',
            speed: 300,
            easing: 'ease',
            offset: 50
        },
        shift: {
            speed: 300,
            easing: 'ease'
        },
        overlap: 150
    }
};

@NgModule({
  declarations: [
    AppComponent,
    LoaderComponent,
    CategoryComponent,
    CategoryBodyComponent,
    GoodListComponent,
    EditGoodComponent,
    EditGoodResetModalContent,
    EditGoodBodyComponent,
    ConnectionComponent,
    BladeHeaderComponent,
    GoodListBodyComponent,
    GoodListConfirmDeleteModalContent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    TreeModule.forRoot(),
    NgxBladeModule,
    NgbModule,
    BrowserAnimationsModule,
    MatProgressBarModule,
    NotifierModule.withConfig(customNotifierOptions)
    ],
  providers: [
    LoaderService,
    { provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true }],
  entryComponents: [
        EditGoodResetModalContent,
        GoodListConfirmDeleteModalContent
    ],
  bootstrap: [AppComponent]
 })
export class AppModule { }
