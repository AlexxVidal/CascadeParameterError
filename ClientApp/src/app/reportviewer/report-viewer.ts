import { Component, Inject, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'report-viewer',
  encapsulation: ViewEncapsulation.None,
  templateUrl: './report-viewer.html',
  styleUrls: [
    "../../../node_modules/devextreme/dist/css/dx.light.css",
    "../../../node_modules/@devexpress/analytics-core/dist/css/dx-analytics.common.css",
    "../../../node_modules/@devexpress/analytics-core/dist/css/dx-analytics.light.css",
    "../../../node_modules/devexpress-reporting/dist/css/dx-webdocumentviewer.css"
  ]
})
export class ReportViewerComponent {
  reportUrl: string = "ReportReport";
  invokeAction: string = '/DXXRDV';

  constructor(@Inject('BASE_URL') public hostUrl: string) { }

  parametersInitialized(event: any): void {
    this.restoreParameters(event.args.ActualParametersInfo, event.args.ParametersModel);
  }

  parametersSubmitted(event: any): void {
    this.persistParameters(event.args.Parameters);
  }

  persistParameters(parameters: any[]): void {
    parameters.forEach((parameter: any) => {
      this.persistParameter(parameter);
    });
  }

  persistParameter(parameter: any): void {
    localStorage.setItem(`${parameter.Key}`, JSON.stringify(parameter));
  }

  restoreParameters(parameters: any[], parametersModel: any): void {
    parameters.forEach((parameter: any) => {
      this.restoreParameter(parameter, parametersModel);
    });
  }

  restoreParameter(parameter: any, parametersModel: any): void {
    if (!parameter.parameterDescriptor?.name) {
      return;
    }

    const persistedValue = localStorage.getItem(`${parameter.parameterDescriptor.name}`);
    if (!persistedValue) {
      return;
    }

    const persistedParameter = JSON.parse(persistedValue);
    if (parameter.parameterDescriptor.type !== persistedParameter.TypeName) {
      return;
    }

    if (parameter.parameterDescriptor.value?.Start || parameter.parameterDescriptor.value?.End) {
      parameter.value(persistedParameter.Value.map((v: any) => new Date(v)));
      return;
    }

    // Solution obtained by support, may break in future versions of Dev-Report.
    // https://supportcenter.devexpress.com/ticket/details/t1137487/persist-and-restore-report-viewer-parameter-values
    if (parameter.parameterDescriptor.multiValue) {
      var multiValueArray = parameter.value();
      if (multiValueArray.value) {
        multiValueArray.value(persistedParameter.Value);
      } else {
        var parameterObject = parametersModel._parameters.filter((x:any) => x.path === parameter.parameterDescriptor.name)[0];
        multiValueArray = parametersModel.parameterHelper.createMultiValueArray(persistedParameter.Value, parameterObject)();
        parameter.value(multiValueArray);
      }

      return;
    }

    parameter.value(persistedParameter.Value);
  }
}