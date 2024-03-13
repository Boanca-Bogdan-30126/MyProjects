clear
close all
clc
load('iddata-09.mat')
u=id.InputData;
y=id.OutputData;

u_id=u;
y_id=y;
u_val=val.InputData;
y_val=val.OutputData;

%modelArx = arx(val,[1 1 1]);
%compare(modelArx, val);
%plot(u)
%figure
%plot(y)

na = 2;
nb = 2;
m = 10;

% Initializare matrici
phi = [];

%initializare minim mse
mse_minim=inf;
mse_minim_predictie=inf;
mse_identificare=[];
mse_predictie=[];

y_simulare_totala=[];
y_predi_total=[];
for i = 1:m
    theta=[];
    phi_id = [];
    phi_val = [];
    %Calculam pentru valoriile de identificare
    for k = 1:length(y_id)
        % pentru y^m
        for lung_na = 1:na
            if k > lung_na
                phi_id(k, lung_na) = -y(k - lung_na)^i;
            else
                phi_id(k, lung_na) = 0;
            end
        end

        % pentru u^m
        for lung_nb = 1:nb
            if k > lung_nb
                phi_id(k, na + lung_nb) = u(k - lung_nb)^i;
            else
                phi_id(k, na + lung_nb) = 0;
            end
        end


        % Combinatia fiecarui termen
        % p reprezinta puterea lui u
        % n reprezinta puterea lui y
        for p=1:m-1
            for n=1:m-1
                for lung_na = 1:na
                    for lung_nb = 1:nb
                        if k > max(lung_na, lung_nb) && (k - lung_nb) > 0 && (k - lung_na) > 0 && p + n <= m
                            phi_id(k, na + nb + lung_na + lung_nb) = u(k - lung_nb)^p * (-y(k - lung_na))^n;
                        else
                            phi_id(k, na + nb + lung_na + lung_nb) = 0;
                        end
                    end

                end
            end
        end
        % Combinatia fiecarui termen

    end
    %Calculam pentru cele de validare
    for k = 1:length(y_val)
        % pentru y^m
        for lung_na = 1:na
            if k > lung_na
                phi_val(k, lung_na) = -y_val(k - lung_na)^i;
            else
                phi_val(k, lung_na) = 0;
            end
        end

        % pentru u^m
        for lung_nb = 1:nb
            if k > lung_nb
                phi_val(k, na + lung_nb) = u_val(k - lung_nb)^i;
            else
                phi_val(k, na + lung_nb) = 0;
            end
        end
        
        % Combinatia fiecarui termen
        % p reprezinta puterea lui u
        % n reprezinta puterea lui y
        for p=1:m-1
            for n=1:m-1
                for lung_na = 1:na
                    for lung_nb = 1:nb
                        if k > max(lung_na, lung_nb) && (k - lung_nb) > 0 && (k - lung_na) > 0 && p + n <= m
                            phi_val(k, na + nb + lung_na + lung_nb) = u_val(k - lung_nb)^p * (-y_val(k - lung_na))^n;
                        else
                            phi_val(k, na + nb + lung_na + lung_nb) = 0;
                        end
                    end

                end
            end
        end
    end

    
    theta=phi_id\y;
    y_predi=phi_val*theta;
    

    
    y_predi_total=[y_predi_total,y_predi];
    y_simulare=zeros(length(u),1);
    y_simulare(1)=0;
    u_simulare(1)=0;

    %Calculam arx-ul prin insumare
    for lungime = 2:length(u)
        Suma = 0;
    
        % Adăugă termenii de la y
        for lung_na = 1:na
            if lungime > lung_na
                Suma = Suma+(-theta(lung_na) * y_simulare(lungime - lung_na));
            end
        end
    
        % Adăugă termenii de la u
        for lung_nb = 1:nb
            if lungime - lung_nb > 0
                Suma = Suma + u(lungime - lung_nb) * theta(na + lung_nb);
            end
        end

        y_simulare(lungime) = Suma;
    end
    y_simulare_totala=[y_simulare_totala,y_simulare];
    %plot(y_simulare)
    %hold on
    %plot(y)
    theta_simulare=phi_id\y_simulare;
    mse_identificare(i)=1/(length(y)*(sum((y-y_simulare).^2)));
    if mse_identificare(i) < mse_minim
        mse_minim=mse_identificare(i);
        y_min=y;
        m_min_grad=i;
    end
    mse_predictie(i)=1/(length(y)*(sum((y-y_predi).^2)));
    if mse_predictie(i)<mse_minim_predictie
        mse_minim_predictie=mse_predictie(i);
        y_min_predi=y;
        m_min_grad_predi=i;
    end


end


figure
plot(y_val)
hold on
plot(y_predi_total(:,1))
title('GRAFICUL PENTRU Y PREDICTIE')

figure

plot(y)
hold on
plot(y_simulare_totala(:,1))
title("Graficul pentru Y SIMULARE")

figure

stem(mse_identificare)
title('Grafic MSE simulare')
figure

stem(mse_predictie)
title("Grafic MSE predictie")


